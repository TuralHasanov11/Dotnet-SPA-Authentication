using System.Text.Json.Serialization;

namespace WebApp.Responses;

public record UserInfo
{
    public string Id { get; }

    public string? Email { get; }

    [JsonPropertyName("username")]
    public string? UserName { get; }
    public IEnumerable<string> Permissions { get; }

    public UserInfo(string id, string? email, string? userName, IEnumerable<string> permissions)
    {
        Id = id;
        Email = email;
        UserName = userName;
        Permissions = permissions;
    }
}