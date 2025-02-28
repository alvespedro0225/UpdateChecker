namespace Domain.Models.JSON;

public sealed class ModelFeed
{
    public string? Result { get; init; }
    public string? Response { get; init; }
    public List<ModelId?> Data { get; init; } = null!;
}