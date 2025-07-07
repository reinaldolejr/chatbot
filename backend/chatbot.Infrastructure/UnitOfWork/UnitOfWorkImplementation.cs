using chatbot.Domain.Interfaces;
using chatbot.Infrastructure.Data;
using chatbot.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace chatbot.Infrastructure.UnitOfWork;

public class UnitOfWorkImplementation : IUnitOfWork
{
    private readonly ChatbotDbContext _context;
    private IDbContextTransaction? _transaction;
    private IChatRepository? _chatRepository;
    private IMessageRepository? _messageRepository;
    private bool _disposed = false;

    public UnitOfWorkImplementation(ChatbotDbContext context)
    {
        _context = context;
    }

    public IChatRepository Chats => _chatRepository ??= new ChatRepository(_context);
    public IMessageRepository Messages => _messageRepository ??= new MessageRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
        _disposed = true;
    }
} 