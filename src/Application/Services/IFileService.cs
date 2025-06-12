namespace Application.Services;

public interface IFileService
{
    public ValueTask<T> GetFileDataAsync<T>(string file);
    public ValueTask SaveCredentialsAsync<T>(string file, T modelCredentials);
}