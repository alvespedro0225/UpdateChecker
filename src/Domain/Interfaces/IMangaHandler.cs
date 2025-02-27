using Data.Models.JSON;

namespace Data.Interfaces
{
    public interface IMangaHandler
    {
        public Task<ModelFeed> GetDataAsync();
        public Task SaveDataAsync(ModelFeed data);
    }
}