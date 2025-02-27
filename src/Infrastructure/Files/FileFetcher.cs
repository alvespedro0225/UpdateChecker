using System.Text.Json;
using Data.Interfaces;

namespace Data.Models;

public sealed class FileFetcher : IJsonFetcher
{
    public async Task<T> GetJsonDataAsync<T>(string file)
    {
        using var stream = new StreamReader(Constants.Path + file);
        var fileData = await stream.ReadToEndAsync();
        var data = JsonSerializer.Deserialize<T>(fileData, Constants.JsonOptions);
        if (data == null) throw new NullReferenceException("File data is null");
        return data;
    }

    public async Task SaveJsonDataAsync<T>(string file, T data)
    {
        await using var stream = new StreamWriter(Constants.Path + file);
        var json = JsonSerializer.Serialize(data, Constants.JsonOptions);
        await stream.WriteAsync(json);
    }
}