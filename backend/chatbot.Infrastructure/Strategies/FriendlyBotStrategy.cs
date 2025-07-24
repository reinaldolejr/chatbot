using chatbot.Domain.Entities;
using chatbot.Domain.Interfaces;

namespace chatbot.Infrastructure.Strategies;

public class FriendlyBotStrategy : IBotStrategy
{
    public string Name => "Friendly";

    public async Task<string> GenerateResponseAsync(Message userMessage, IEnumerable<Message> conversationHistory)
    {
        var userInput = userMessage.Content.ToLower();
        
        // answers based on keywords
        if (userInput.Contains("hello") || userInput.Contains("hi") || userInput.Contains("hey"))
        {
            return "Hello! I'm very happy to chat with you! How are you today?";
        }
        
        if (userInput.Contains("how are you"))
        {
            return "I'm fantastic, thanks for asking! I hope you're having a wonderful day too!";
        }
        
        if (userInput.Contains("help") || userInput.Contains("support"))
        {
            return "I'm absolutely happy to help you! What can I do for you today?";
        }
        
        if (userInput.Contains("thank you"))
        {
            return "You're welcome! I'm very happy to help. Is there anything else you'd like to talk about?";
        }
        
        if (userInput.Contains("bye") )
        {
            return "Goodbye! It was a pleasure chatting with you! Come back anytime - I'll be here with a smile!";
        }
        
        // default answers if there is no answer for the user message
        var responses = new[]
        {
            "That's really interesting! Tell me more about it!",
            "I love how you think! What made you think about that?",
            "That's a great point! I'd love to hear your thoughts on it.",
            "You're absolutely right! I'm really enjoying our conversation!",
            "That's fascinating! I'm learning a lot talking with you!"
        };
        
        var random = new Random();
        return responses[random.Next(responses.Length)];
    }

} 