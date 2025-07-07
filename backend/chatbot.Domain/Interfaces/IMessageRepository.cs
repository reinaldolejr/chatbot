using chatbot.Domain.Entities;

namespace chatbot.Domain.Interfaces;

public interface IMessageRepository : IRepository<Message>
{
    Task<IEnumerable<Message>> GetMessagesByChatIdAsync(Guid chatId);
} 