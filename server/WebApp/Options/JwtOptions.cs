using System.ComponentModel.DataAnnotations;

namespace WebApp.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    [Required]
    public string Secret { get; set; } = string.Empty;

    [Required]
    public string ValidIssuer { get; set; } = string.Empty;

    [Required]
    public string ValidAudience { get; set; } = string.Empty;

    [Required]
    [Range(1, 500)]
    public int AccessTokenLifeTime { get; set; }

    [Required]
    [Range(1, 500)]
    public int RefreshTokenLifeTime { get; set; }
}