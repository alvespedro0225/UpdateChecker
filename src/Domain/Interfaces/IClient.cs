namespace Data.Interfaces;

public interface IClient
{
    public Task<string> CheckFeedAsync();
}