using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApp.Models.Configuration;

public class ApplicationUserTokenConfiguration : IEntityTypeConfiguration<ApplicationUserToken>
{
    private const string _tableName = "ApplicationUserTokens";

    public void Configure(EntityTypeBuilder<ApplicationUserToken> builder)
    {
        builder.ToTable(_tableName);
    }
}