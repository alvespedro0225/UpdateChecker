using System.Text.Json;
using Domain.Interfaces;
using Domain.Models;

namespace Infrastructure.Files;

public sealed class FileFetcher : IJsonFetcher
{
    public async Task<T> GetJsonDataAsync<T>(string file)
    {
        var path = Path.Combine(Constants.Path, file);
        if (!File.Exists(path))
        {
            Console.WriteLine($"File {path} does not exist");
            Environment.Exit(1);
        }

        using var stream = new StreamReader(path);
        var fileData = await stream.ReadToEndAsync();
        var data = JsonSerializer.Deserialize<T>(fileData, Constants.JsonOptions);
        if (data == null) throw new NullReferenceException("File data is null");
        return data;
    }

    public async Task SaveJsonDataAsync<T>(string file, T data)
    {
        var path = Path.Combine(Constants.Path, file);
        await using var stream = new StreamWriter(path);
        var json = JsonSerializer.Serialize(data, Constants.JsonOptions);
        await stream.WriteAsync(json);
    }
}