using chatbot.Domain.Entities;
using chatbot.Domain.Interfaces;
using System.Collections.Concurrent;

namespace chatbot.Infrastructure.Observers;

public class NotificationSubject : ISubject
{
    private readonly ConcurrentDictionary<string, IObserver> _observers = new();
    private readonly ConcurrentDictionary<string, List<string>> _chatConnections = new();

    public int ObserverCount => _observers.Count;

    public void Attach(IObserver observer)
    {
        if (observer is WebSocketObserver wsObserver)
        {
            _observers.TryAdd(observer.ConnectionId, observer);
            var chatId = wsObserver.ChatId;
            if (!string.IsNullOrEmpty(chatId))
            {
                _chatConnections.AddOrUpdate(
                    chatId,
                    new List<string> { observer.ConnectionId },
                    (key, existing) =>
                    {
                        if (!existing.Contains(observer.ConnectionId))
                            existing.Add(observer.ConnectionId);
                        return existing;
                    });
            }
        }
    }

    public void Detach(IObserver observer)
    {
        if (_observers.TryRemove(observer.ConnectionId, out _))
        {
            if (observer is WebSocketObserver wsObserver)
            {
                var chatId = wsObserver.ChatId;
                if (!string.IsNullOrEmpty(chatId) && _chatConnections.TryGetValue(chatId, out var connections))
                {
                    connections.Remove(observer.ConnectionId);
                    if (connections.Count == 0)
                    {
                        _chatConnections.TryRemove(chatId, out _);
                    }
                }
            }
        }
    }

    public async Task NotifyObserversAsync(NotificationMessage message)
    {
        var tasks = new List<Task>();

        foreach (var observer in _observers.Values)
        {
            tasks.Add(observer.NotifyAsync(message));
        }
        await Task.WhenAll(tasks);
    }

    public async Task NotifyObserverAsync(string chatId, NotificationMessage message)
    {
        if (_chatConnections.TryGetValue(chatId, out var connectionIds))
        {
            var tasks = connectionIds
                .Where(id => _observers.TryGetValue(id, out var observer))
                .Select(id => _observers[id].NotifyAsync(message));
            await Task.WhenAll(tasks);
        }
    }

    public IEnumerable<IObserver> GetActiveObservers()
    {
        return _observers.Values;
    }

} 