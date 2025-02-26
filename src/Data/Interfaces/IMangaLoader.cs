using Data.Models;

namespace Data.Interfaces
{
    public interface IMangaLoader
    {
        public Task<ModelFeed> GetDataAsync();
        public Task SaveDataAsync(ModelFeed data);
    }
}