using System.Text.Json;

namespace Domain.Constants;

public static class JsonOptions
{
    public static JsonSerializerOptions Default { get; } = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
        IndentSize = 4
    };

    public static JsonSerializerOptions SnakeCase { get; } = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
        IndentSize = 4,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };
}