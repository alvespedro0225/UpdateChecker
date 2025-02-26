using updateBot.Models;

namespace updateBot.Interfaces;

public interface ICredentialsHandler
{
    public Task<ModelCredentials> GetCredentialsAsync();
    public Task SaveCredentialsAsync(ModelCredentials modelCredentials);
}