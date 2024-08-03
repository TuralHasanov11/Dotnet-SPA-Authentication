using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApp.Models.Configuration;

public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
{
    private const string _tableName = "ApplicationUserRoles";

    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
    {
        builder.ToTable(_tableName);
    }
}