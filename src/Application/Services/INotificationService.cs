namespace Application.Services;

public interface INotificationService
{
    public Task Notify(string message);
}