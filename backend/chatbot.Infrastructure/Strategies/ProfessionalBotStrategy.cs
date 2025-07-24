using chatbot.Domain.Entities;
using chatbot.Domain.Interfaces;

namespace chatbot.Infrastructure.Strategies;

public class ProfessionalBotStrategy : IBotStrategy
{
    public string Name => "Professional";

    public async Task<string> GenerateResponseAsync(Message userMessage, IEnumerable<Message> conversationHistory)
    {
        var userInput = userMessage.Content.ToLower();
        
        // answers based on keywords
        if (userInput.Contains("hello") || userInput.Contains("hi") || userInput.Contains("hey"))
        {
            return "Hello. I am here to assist you with any inquiries you may have. How can I help you today?";
        }
        
        if (userInput.Contains("how are you"))
        {
            return "I am functioning perfectly, thank you for asking. How can I assist you today?";
        }
        
        if (userInput.Contains("help") || userInput.Contains("support"))
        {
            return "I am available to provide assistance. Please specify the nature of your inquiry so I can better assist you.";
        }
        
        if (userInput.Contains("thank you"))
        {
            return "You're welcome. I am here to help. Is there anything else you need?";
        }
        
        if (userInput.Contains("meeting") || userInput.Contains("schedule"))
        {
            return "I can assist with scheduling. Please provide the details of your meeting requirements.";
        }
        
        if (userInput.Contains("report") || userInput.Contains("data"))
        {
            return "I can assist with data analysis and report generation. What specific information are you looking for?";
        }
        
        if (userInput.Contains("bye"))
        {
            return "Goodbye. Thank you for your time. Have a productive day.";
        }
        
        // default answers if there is no answer for the user message
        var responses = new[]
        {
            "I understand your inquiry. I will provide the relevant information.",
            "Thank you for your message. I am processing your request and will respond as necessary.",
            "Thank you for your input. How would you like to proceed with this matter?",
            "Your message has been received. I am here to help with any further questions.",
            "Thank you for your communication. Please let me know if you need additional support."
        };
        
        var random = new Random();
        return responses[random.Next(responses.Length)];
    }

} 