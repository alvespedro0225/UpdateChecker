using System.Text.Json;

namespace Domain.Constants;

public static class JsonOptions
{
    public static JsonSerializerOptions Options { get; } = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
        IndentSize = 4
    };
}