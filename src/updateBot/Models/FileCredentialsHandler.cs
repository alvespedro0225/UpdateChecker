using System.Text.Json;
using updateBot.Interfaces;

namespace updateBot.Models;

public sealed class FileCredentialsHandler(string path) : ICredentialsHandler
{
    public async Task<ModelCredentials> GetCredentialsAsync()
    {
        using var stream = new StreamReader(path);
        var data = await stream.ReadToEndAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var jsonData = JsonSerializer.Deserialize<ModelCredentials>(data, options);
        return jsonData ?? throw new NullReferenceException("Null data returned");
        
    }

    public async Task SaveCredentialsAsync(ModelCredentials content)
    {
        await using var stream = new StreamWriter(path);
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            IndentSize = 4
        };
        var jsonData = JsonSerializer.Serialize(content, options);
        if (string.IsNullOrEmpty(jsonData)) throw new NullReferenceException("Null data returned");
        await stream.WriteAsync(jsonData);
    }
}