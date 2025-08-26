namespace Application.Services;

public interface IFileService
{
    public ValueTask<T> GetFileDataAsync<T>(string file);
    public ValueTask SaveFileDataAsync<T>(string file, T modelCredentials);
    public T GetFileData<T>(string file);
}