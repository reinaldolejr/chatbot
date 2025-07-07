using chatbot.Domain.Entities;

namespace chatbot.Domain.Interfaces;

public interface IBotService
{
    Task<string> GetBotResponseAsync(Guid chatId, string userMessage, string sender);
    
} 