namespace WebApp.Abstractions;

public interface IAuditableEntity
{
    public DateTime CreatedOnUtc { get; }

    public DateTime? UpdatedOnUtc { get; }

    public byte[]? RowVersion { get; }
}