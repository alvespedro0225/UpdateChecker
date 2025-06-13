namespace Domain.Constants;

public static class Directories
{
    public static readonly string BaseDir =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DirectoryName);
    private const string DirectoryName = "updateChecker";
    public static string CredentialsFile =>  BaseDir + "credentials.json";
    public static string MailFile => BaseDir + "mailData.json";
    public static string FeedFile => BaseDir + "mangas.json";
    public static string Subject => BaseDir +  "New chapters";
}