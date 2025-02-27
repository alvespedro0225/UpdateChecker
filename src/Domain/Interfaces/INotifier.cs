namespace Data.Interfaces;

public interface INotifier
{
    public Task Notify(string message);
}