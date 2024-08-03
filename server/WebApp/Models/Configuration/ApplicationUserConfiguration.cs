using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApp.Models.Configuration;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    private const string _tableName = "ApplicationUsers";

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable(_tableName);

        builder.HasMany(u => u.Claims)
            .WithOne()
            .HasForeignKey(c => c.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Logins)
            .WithOne()
            .HasForeignKey(l => l.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Tokens)
            .WithOne()
            .HasForeignKey(t => t.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.UserRoles)
            .WithOne()
            .HasForeignKey(ur => ur.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}