using Domain.Models;

namespace Application.Services;

public interface IFeedService
{
    bool CheckUpdate(ModelFeed staleData, ModelFeed freshData, out List<string> newData);
}