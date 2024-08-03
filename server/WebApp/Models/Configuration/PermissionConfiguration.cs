using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApp.Models.Configuration;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    private const string _tableName = "Permissions";

    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(_tableName);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired();
    }
}