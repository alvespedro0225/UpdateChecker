namespace Domain.Models.JSON;

public sealed class ModelMailInfo
{
    public required string FromName { get; set; }
    public required string ToName { get; set; }
    public required string From { get; set; }
    public required string To { get; set; }
    public required string Password { get; set; }
    public required string Host { get; set; }
    public required int Port { get; set; }
}