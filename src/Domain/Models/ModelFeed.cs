namespace Domain.Models;

public sealed record ModelFeed
{
    public required string Result { get; init; }
    public required string Response { get; init; }
    public required List<ModelId> Data { get; init; }
}