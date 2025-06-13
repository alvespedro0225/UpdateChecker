using Domain.Models;

namespace Application.Services;

public interface IMangadexService
{
    public ValueTask<ModelFeed?> GetAsync();
    bool CheckUpdate(ModelFeed staleData, ModelFeed freshData, out List<string> newData);
}