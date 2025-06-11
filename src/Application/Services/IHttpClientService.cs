namespace Application.Services;

public interface IHttpClientService
{
    public Task<string> GetAsync();
}