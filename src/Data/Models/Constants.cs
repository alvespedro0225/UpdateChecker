using System.Text.Json;

namespace Data.Models;

public static class Constants
{
    public const string MangaFile = "mangas.json";
    public const string CredentialsFile = "credentials.json";
    public const string Path = "/home/pedro/code/csharp/updateBot/src/Data/";
    public static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
        IndentSize = 4
    };
}