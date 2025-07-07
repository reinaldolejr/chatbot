namespace chatbot.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IChatRepository Chats { get; }
    IMessageRepository Messages { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
