using Domain.Models.JSON;

namespace Domain.Interfaces
{
    public interface IMangaHandler
    {
        public Task<ModelFeed> GetDataAsync();
        public Task SaveDataAsync(ModelFeed data);
    }
}