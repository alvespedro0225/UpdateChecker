using System.Text.Json;

namespace Data.Models;

public static class FileManager
{
    public static async Task<T> GetJsonFileAsync<T>(string file)
    {
        using var stream = new StreamReader(Constants.Path + file);
        var fileData = await stream.ReadToEndAsync();
        var data = JsonSerializer.Deserialize<T>(fileData, Constants.JsonOptions);
        if (data == null) throw new NullReferenceException("File data is null");
        return data;
    }

    public static async Task SaveJsonFileAsync<T>(string file, T data)
    {
        await using var stream = new StreamWriter(Constants.Path + file);
        var json = JsonSerializer.Serialize(data, Constants.JsonOptions);
        await stream.WriteAsync(json);
    }
}