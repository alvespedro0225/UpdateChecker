namespace Domain.Interfaces;

public interface IClient
{
    public Task<string> CheckFeedAsync();
}