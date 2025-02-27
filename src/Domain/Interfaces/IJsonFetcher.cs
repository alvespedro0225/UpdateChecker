namespace Data.Interfaces;

public interface IJsonFetcher
{
    public Task<T> GetJsonDataAsync<T>(string file);
    public Task SaveJsonDataAsync<T>(string file, T data);
}