using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApp.Models.Configuration;

public class ApplicationRolePermissionConfiguration : IEntityTypeConfiguration<ApplicationRolePermission>
{
    private const string _tableName = "ApplicationRolePermission";

    public void Configure(EntityTypeBuilder<ApplicationRolePermission> builder)
    {
        builder.ToTable(_tableName);

        builder.HasKey(rp => new { rp.ApplicationRoleId, rp.PermissionId });
    }
}