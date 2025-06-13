using System.Text.Json;
using Application.Services;
using Domain.Constants;

namespace Infrastructure.Files;

public sealed class FileService : IFileService
{
    public async ValueTask<T> GetFileDataAsync<T>(string file)
    {
        var path = Path.Combine(Directories.BaseDir, file);
        
        if (!File.Exists(path))
        {
            Console.WriteLine($"File {path} does not exist");
            Environment.Exit(1);
        }

        using var stream = new StreamReader(path);
        var fileData = await stream.ReadToEndAsync();
        var data = JsonSerializer.Deserialize<T>(fileData, JsonOptions.Options);
        
        if (data is null) 
            throw new NullReferenceException("File data is null");
        
        return data;
    }
    
    public async ValueTask SaveCredentialsAsync<T>(string file, T data)
    {
        var path = Path.Combine(Directories.BaseDir, file);
        await using var stream = new StreamWriter(path);
        var json = JsonSerializer.Serialize(data, JsonOptions.Options);
        await stream.WriteAsync(json);
    }

    public T GetFileData<T>(string file)
    {
        var path = Path.Combine(Directories.BaseDir, file);
        
        if (!File.Exists(path))
        {
            Console.WriteLine($"File {path} does not exist");
            Environment.Exit(1);
        }

        using var stream = new StreamReader(path);
        var fileData = stream.Read();
        var data = JsonSerializer.Deserialize<T>(fileData, JsonOptions.Options);
        
        if (data is null) 
            throw new NullReferenceException("File data is null");
        
        return data;
    }
}