using Microsoft.EntityFrameworkCore;
using WebApp.Abstractions;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Services;

public class RefreshTokenRepository(ApplicationDbContext dbContext) : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task CreateRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        return await _dbContext.RefreshTokens
            .SingleOrDefaultAsync(t => t.Token == Guid.Parse(token));
    }

    public async Task<RefreshToken?> GetRefreshTokenByJtiAsync(string jti)
    {
        return await _dbContext.RefreshTokens
           .SingleOrDefaultAsync(t => t.JwtId == jti);
    }

    public void UpdateRefreshToken(RefreshToken refreshToken)
    {
        _dbContext.RefreshTokens.Update(refreshToken);
    }
}