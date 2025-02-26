namespace updateBot.Models;

public sealed class ModelCredentials
{
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
}