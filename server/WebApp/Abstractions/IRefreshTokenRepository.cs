using WebApp.Models;

namespace WebApp.Abstractions;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetRefreshTokenAsync(string token);

    Task<RefreshToken?> GetRefreshTokenByJtiAsync(string jti);

    Task CreateRefreshTokenAsync(RefreshToken refreshToken);

    void UpdateRefreshToken(RefreshToken refreshToken);
}