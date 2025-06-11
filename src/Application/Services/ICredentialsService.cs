namespace Application.Services;

public interface ICredentialsService
{
    public Task<T> GetCredentialsAsync<T>(string file);
    public Task SaveCredentialsAsync<T>(string file, T modelCredentials);
}