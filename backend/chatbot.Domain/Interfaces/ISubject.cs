using chatbot.Domain.Entities;

namespace chatbot.Domain.Interfaces;

public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    Task NotifyObserversAsync(NotificationMessage message);
    Task NotifyObserverAsync(string observerId, NotificationMessage message);
    IEnumerable<IObserver> GetActiveObservers();
    int ObserverCount { get; }
} 