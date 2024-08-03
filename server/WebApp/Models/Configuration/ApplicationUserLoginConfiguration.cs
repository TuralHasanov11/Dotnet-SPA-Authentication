using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApp.Models.Configuration;

public class ApplicationUserLoginConfiguration : IEntityTypeConfiguration<ApplicationUserLogin>
{
    private const string _tableName = "ApplicationUserLogins";

    public void Configure(EntityTypeBuilder<ApplicationUserLogin> builder)
    {
        builder.ToTable(_tableName);
    }
}