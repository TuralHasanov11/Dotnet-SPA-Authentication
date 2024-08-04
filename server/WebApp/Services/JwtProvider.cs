using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApp.Abstractions;
using WebApp.Data;
using WebApp.Identity;
using WebApp.Models;
using WebApp.Models.Shared;
using WebApp.Options;

namespace WebApp.Services;

public class JwtProvider(
    IOptions<JwtOptions> tokenOptions,
    TokenValidationParameters tokenValidationParameters,
    IRefreshTokenRepository refreshTokenRepository,
    ApplicationDbContext dbContext) : IJwtProvider
{
    private readonly JwtOptions _tokenOptions = tokenOptions.Value;
    private readonly TokenValidationParameters _tokenValidationParameters = tokenValidationParameters;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Result<AccessTokenResponse>> GenerateToken(ClaimsPrincipal principal)
    {
        var claims = new List<Claim>();

        var userId = principal.GetUserId();

        claims.Add(new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new(JwtRegisteredClaimNames.Sub, userId.ToString()));
        claims.Add(new(JwtRegisteredClaimNames.Email, principal.GetUserEmail()));
        claims.Add(new(ApplicationClaimTypes.Id, userId.ToString()));

        var handler = new JwtSecurityTokenHandler();

        var token = handler.CreateToken(new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.Secret)),
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = _tokenOptions.ValidIssuer,
            Audience = _tokenOptions.ValidAudience,
            Expires = DateTime.UtcNow.AddSeconds(_tokenOptions.AccessTokenLifeTime),
        });

        var refreshToken = new RefreshToken(
            token.Id,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(_tokenOptions.RefreshTokenLifeTime),
            userId);

        await _refreshTokenRepository.CreateRefreshTokenAsync(refreshToken);
        await _dbContext.SaveChangesAsync();

        return Result.Success(new AccessTokenResponse
        {
            AccessToken = handler.WriteToken(token),
            RefreshToken = refreshToken.Token.ToString(),
            ExpiresIn = _tokenOptions.AccessTokenLifeTime * 60,
        });
    }

    public async Task<Result> RevokeToken(ClaimsPrincipal principal)
    {
        var jti = principal.Claims
                .Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

        var storedRefreshToken = await _refreshTokenRepository.GetRefreshTokenByJtiAsync(jti);

        if (storedRefreshToken is not null)
        {
            storedRefreshToken.MarkAsUsed();
            storedRefreshToken.Invalidate();
            _refreshTokenRepository.UpdateRefreshToken(storedRefreshToken);
            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }

        return Result.Failure(
            Error.Validation("RefreshToken.NotFound", "Refresh Token not found"));
    }

    public async Task<Result<AccessTokenResponse>> RefreshToken(string accessToken, string refreshToken)
    {
        var result = await GetPrincipalFromToken(accessToken, refreshToken);

        if (result.IsFailure)
        {
            return Result.Failure<AccessTokenResponse>(result.Error);
        }

        return await GenerateToken(result.Value!);
    }

    private async Task<Result<ClaimsPrincipal>> GetPrincipalFromToken(string accessToken, string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var validationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = _tokenValidationParameters.ValidateIssuer,
                ValidateAudience = _tokenValidationParameters.ValidateAudience,
                ValidAudience = _tokenValidationParameters.ValidAudience,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = _tokenValidationParameters.ValidateIssuerSigningKey,
                ValidIssuer = _tokenValidationParameters.ValidIssuer,
                IssuerSigningKey = _tokenValidationParameters.IssuerSigningKey,
                ClockSkew = _tokenValidationParameters.ClockSkew
            };

            var principal = tokenHandler.ValidateToken(
                accessToken,
                validationParameters,
                out var validatedToken);

            var jti = principal.Claims
                .Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _refreshTokenRepository.GetRefreshTokenAsync(refreshToken);

            if (storedRefreshToken?.IsActive() is false || storedRefreshToken?.JwtId != jti)
            {
                return Result.Failure<ClaimsPrincipal>(
                    Error.Validation("RefreshToken.Invalid", "Invalid Refresh Token"));
            }

            storedRefreshToken.MarkAsUsed();

            _refreshTokenRepository.UpdateRefreshToken(storedRefreshToken);
            await _dbContext.SaveChangesAsync();

            return Result.Success(principal);
        }
        catch (Exception ex)
        {
            return Result.Failure<ClaimsPrincipal>(
                Error.Failure("GetPrincipalFromToken.Failure", ex.Message));
        }
    }
}