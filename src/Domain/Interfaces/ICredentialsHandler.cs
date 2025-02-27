using Data.Models.JSON;

namespace Data.Interfaces;

public interface ICredentialsHandler
{
    public Task<ModelCredentials> GetCredentialsAsync();
    public Task SaveCredentialsAsync(ModelCredentials modelCredentials);
}