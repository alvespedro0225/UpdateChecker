namespace Domain.Models;

public sealed record ModelCredentials
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
}