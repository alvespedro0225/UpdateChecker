using Domain.Models.JSON;

namespace Domain.Interfaces;

public interface ICredentialsHandler
{
    public Task<ModelCredentials> GetCredentialsAsync();
    public Task SaveCredentialsAsync(ModelCredentials modelCredentials);
}