using System.Text.Json;
using updateBot.Interfaces;

namespace updateBot.Models;

public sealed class FileMangaLoader(string path, JsonSerializerOptions options) : IMangaLoader
{
    public async Task<ModelFeed> GetDataAsync()
    {
        using var stream = new StreamReader(path);
        var data = await JsonSerializer.DeserializeAsync<ModelFeed>(stream.BaseStream, options);
        return data ?? throw new NullReferenceException();
    }

    public async Task SaveDataAsync(ModelFeed data)
    {
        var json = JsonSerializer.Serialize(data, options);
        await using var stream = new StreamWriter(path);
        await stream.WriteAsync(json);
    }
}