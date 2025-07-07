using chatbot.Domain.Entities;
using chatbot.Domain.Interfaces;
using System.Collections.Concurrent;
using chatbot.Infrastructure.Observers;

namespace chatbot.Infrastructure.Observers;

public class NotificationService : INotificationService
{
    private readonly ISubject _subject;
    private readonly ConcurrentDictionary<string, IObserver> _chatObservers = new();
    private readonly ConcurrentDictionary<string, List<NotificationMessage>> _userNotifications = new();

    public NotificationService(ISubject subject)
    {
        _subject = subject;
    }

    public async Task SendNotificationAsync(NotificationMessage message)
    {
        await _subject.NotifyObserversAsync(message);
    }

    public async Task<IObserver> RegisterObserverAsync(string chatId, string connectionId)
    {
        var observer = new WebSocketObserver(chatId, connectionId);
        _chatObservers.TryAdd(connectionId, observer);
        _subject.Attach(observer);
        return await Task.FromResult(observer);
    }

    public async Task UnregisterObserverAsync(string connectionId)
    {
        if (_chatObservers.TryRemove(connectionId, out var observer))
        {
            _subject.Detach(observer);
            if (observer is WebSocketObserver wsObserver)
            {
                wsObserver.Complete();
            }
        }
        await Task.CompletedTask;
    }

    public async Task<IEnumerable<NotificationMessage>> GetUserNotificationsAsync(string userId, int limit = 50)
    {
        if (_userNotifications.TryGetValue(userId, out var notifications))
        {
            return await Task.FromResult(notifications.Take(limit));
        }
        return await Task.FromResult(Enumerable.Empty<NotificationMessage>());
    }

}