namespace WebApp.Models;

public class RefreshToken
{
    public Guid Token { get; }

    public string JwtId { get; }

    public DateTime CreationDate { get; }

    public DateTime ExpiryDate { get; }

    public bool Used { get; private set; } = false;

    public bool Invalidated { get; private set; } = false;

    public Guid UserId { get; }

    public ApplicationUser User { get; }

    public RefreshToken(
        string jwtId,
        DateTime creationDate,
        DateTime expiryDate,
        Guid userId)
    {
        Token = Guid.NewGuid();
        JwtId = jwtId;
        CreationDate = creationDate;
        ExpiryDate = expiryDate;
        UserId = userId;
    }

    private RefreshToken()
    {
    }

    public void Invalidate()
    {
        Invalidated = true;
    }

    public void MarkAsUsed()
    {
        Used = true;
    }

    public bool IsExpired() => DateTime.UtcNow >= ExpiryDate;

    public bool IsActive() => !Used && !Invalidated && !IsExpired();
}
