using Domain.Enums;

namespace Application.Services;

public interface INotificationService
{
    public Task Notify(string message);
    Task NotifyError(Error error);
}