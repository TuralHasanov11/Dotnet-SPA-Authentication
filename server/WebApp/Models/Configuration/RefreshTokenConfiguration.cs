using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApp.Models.Configuration;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    private const string _tableName = "RefreshTokens";

    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable(_tableName);

        builder.HasKey(x => x.Token);

        builder.Property(x => x.ExpiryDate).IsRequired();
        builder.Property(x => x.CreationDate).IsRequired();
        builder.Property(x => x.JwtId).IsRequired();
        builder.Property(x => x.Invalidated).HasDefaultValue(false);
        builder.Property(x => x.Used).HasDefaultValue(false);

        builder.HasIndex(x => x.UserId);
        builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
    }
}