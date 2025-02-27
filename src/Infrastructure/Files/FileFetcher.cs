using System.Text.Json;
using Data.Interfaces;
using Data.Models;

namespace Infrastructure.Files;

public sealed class FileFetcher : IJsonFetcher
{
    public async Task<T> GetJsonDataAsync<T>(string file)
    {
        if (!File.Exists(Constants.Path + file))
        {
            Console.WriteLine($"File {file} does not exist");
            Environment.Exit(1);
        }
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