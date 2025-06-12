using Domain.Models;

namespace Application.Services;

public interface IHttpClientService
{
    public ValueTask<ModelFeed> GetAsync();
}