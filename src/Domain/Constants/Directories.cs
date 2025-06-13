namespace Domain.Constants;

public static class Directories
{
    public static readonly string BaseDir =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DirectoryName);
    private const string DirectoryName = "updateChecker";
    public static string CredentialsFile =>  Path.Combine(BaseDir, "credentials.json");
    public static string MailFile => Path.Combine(BaseDir, "mailData.json");
    public static string FeedFile => Path.Combine(BaseDir, "mangas.json");
}