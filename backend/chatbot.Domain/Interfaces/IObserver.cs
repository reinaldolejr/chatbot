using chatbot.Domain.Entities;

namespace chatbot.Domain.Interfaces;

public interface IObserver
{
    string Id { get; }
    string ConnectionId { get; }
    string ChatId { get; }
    Task NotifyAsync(NotificationMessage message);
    bool IsActive { get; }
    DateTime LastActivity { get; }
}
