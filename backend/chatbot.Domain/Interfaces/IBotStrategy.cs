using chatbot.Domain.Entities;

namespace chatbot.Domain.Interfaces;

public interface IBotStrategy
{
    string Name { get; }
    Task<string> GenerateResponseAsync(Message userMessage, IEnumerable<Message> conversationHistory);
    
} 