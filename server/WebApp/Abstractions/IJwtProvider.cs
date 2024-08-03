using Microsoft.AspNetCore.Authentication.BearerToken;
using System.Security.Claims;
using WebApp.Models;
using WebApp.Models.Shared;

namespace WebApp.Abstractions;

public interface IJwtProvider
{
    Task<Result<AccessTokenResponse>> GenerateToken(ClaimsPrincipal principal);

    Task<Result> RevokeToken(ClaimsPrincipal principal);

    Task<Result<AccessTokenResponse>> RefreshToken(string accessToken, string refreshToken);
}