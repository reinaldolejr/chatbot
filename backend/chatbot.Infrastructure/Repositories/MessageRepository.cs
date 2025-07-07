using chatbot.Domain.Entities;
using chatbot.Domain.Interfaces;
using chatbot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace chatbot.Infrastructure.Repositories;

public class MessageRepository : Repository<Message>, IMessageRepository
{
    public MessageRepository(ChatbotDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Message>> GetMessagesByChatIdAsync(Guid chatId)
    {
        return await _dbSet
            .Where(m => m.ChatId == chatId)
            .OrderBy(m => m.Timestamp)
            .ToListAsync();
    }

} 