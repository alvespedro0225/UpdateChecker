namespace Domain.Models;

public sealed record ModelMailInfo
{
    public required string FromName { get; init; }
    public required string ToName { get; init; }
    public required string From { get; init; }
    public required string To { get; init; }
    public required string Password { get; init; }
    public required string Host { get; init; }
    public required int Port { get; init; }
}