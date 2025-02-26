using updateBot.Models;

namespace updateBot.Interfaces;

public interface IMangaLoader
{
    public Task<ModelFeed> GetDataAsync();
    public Task SaveDataAsync(ModelFeed data);
}