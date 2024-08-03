using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApp.Models.Configuration;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    private const string _tableName = "ApplicationRoles";

    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.ToTable(_tableName);

        builder.HasMany(r => r.UserRoles)
            .WithOne()
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.RoleClaims)
            .WithOne()
            .HasForeignKey(rc => rc.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.Permissions)
            .WithMany()
            .UsingEntity<ApplicationRolePermission>();
    }
}