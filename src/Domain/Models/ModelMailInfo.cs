namespace Domain.Models;

public sealed record ModelMailInfo
{
    public required string ReceiverName { get; init; }
    public required string ReceiverEmail { get; init; }
    public required string SenderEmail { get; init; }
    public required string SenderPassword { get; init; }
    public required string Host { get; init; }
    public required int Port { get; init; }
}