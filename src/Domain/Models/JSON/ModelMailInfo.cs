namespace Domain.Models.JSON;

public sealed class ModelMailInfo
{
    public string FromName { get; set; } = null!;
    public string ToName { get; set; } = null!;
    public string From { get; set; } = null!;
    public string To { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Host { get; set; } = null!;
    public int Port { get; set; }
}