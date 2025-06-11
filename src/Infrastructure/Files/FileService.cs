using System.Text.Json;
using Application.Services;
using Domain.Models;

namespace Infrastructure.Files;

public sealed class FileService : ICredentialsService
{
    public async Task<T> GetCredentialsAsync<T>(string file)
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
        
        if (data is null) 
            throw new NullReferenceException("File data is null");
        
        return data;
    }

    public Task<T> GetCredentialsAsync<T>()
    {
        throw new NotImplementedException();
    }

    public async Task SaveCredentialsAsync<T>(string file, T data)
    {
        var path = Path.Combine(Constants.Path, file);
        await using var stream = new StreamWriter(path);
        var json = JsonSerializer.Serialize(data, Constants.JsonOptions);
        await stream.WriteAsync(json);
    }
}