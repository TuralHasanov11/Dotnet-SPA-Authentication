using System.Text.Json.Serialization;

namespace WebApp.Requests;

public sealed record RefreshTokenRequest
{
    public string AccessToken { get; init; }

    public string RefreshToken { get; init; }
}