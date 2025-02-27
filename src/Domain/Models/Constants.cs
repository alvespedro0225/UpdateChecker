using System.Text.Json;

namespace Data.Models;

public static class Constants
{
    public const string FeedFile = "mangas.json";
    public const string CredentialsFile = "credentials.json";
    public const string MangadexFeed = "/user/follows/manga/feed";
    public const string MangadexApi = "https://api.mangadex.org";
    public const string MangadexAuth = "https://auth.mangadex.org/realms/mangadex/protocol/openid-connect/token";
    public const string MailFile = "mailData.json";
    public const string Subject = "New chapters";
    public const string ChapterUrl = "https://mangadex.org/chapter";

    public static readonly string Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
                                         "/.updateBot/";

    public static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true, WriteIndented = true, IndentSize = 4
    };
}