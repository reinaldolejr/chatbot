using chatbot.Domain.Entities;

namespace chatbot.Domain.Interfaces;

public interface INotificationService
{
    Task SendNotificationAsync(NotificationMessage message);
    Task<IObserver> RegisterObserverAsync(string userId, string connectionId);
    Task UnregisterObserverAsync(string connectionId);
    Task<IEnumerable<NotificationMessage>> GetUserNotificationsAsync(string userId, int limit = 50);
    
} 