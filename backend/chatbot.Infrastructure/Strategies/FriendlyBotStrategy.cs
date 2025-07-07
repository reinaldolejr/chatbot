using chatbot.Domain.Entities;
using chatbot.Domain.Interfaces;

namespace chatbot.Infrastructure.Strategies;

public class FriendlyBotStrategy : IBotStrategy
{
    public string Name => "Amigável";

    public async Task<string> GenerateResponseAsync(Message userMessage, IEnumerable<Message> conversationHistory)
    {
        var userInput = userMessage.Content.ToLower();
        
        // respostas baseadas em palavras-chave
        if (userInput.Contains("olá") || userInput.Contains("oi") || userInput.Contains("eai"))
        {
            return "Olá! Estou muito feliz em conversar com você! Como você está hoje?";
        }
        
        if (userInput.Contains("como você está"))
        {
            return "Estou fantástico, obrigado por perguntar! Espero que você esteja tendo um dia maravilhoso também!";
        }
        
        if (userInput.Contains("ajuda") || userInput.Contains("suporte"))
        {
            return "Estou absolutamente feliz em ajudar você! O que posso fazer por você hoje?";
        }
        
        if (userInput.Contains("obrigado"))
        {
            return "De nada! Estou muito feliz em ajudar. Há algo mais que você gostaria de conversar?";
        }
        
        if (userInput.Contains("sair") )
        {
            return "Adeus! Foi um prazer conversar com você! Volte sempre - estarei aqui com um sorriso!";
        }
        
        // respostas padrão se não houver resposta para a mensagem do usuário
        var responses = new[]
        {
            "Isso é realmente interessante! Me diga mais sobre isso!",
            "Adoro como você pensa! O que fez você pensar sobre isso?",
            "Isso é um ótimo ponto! Gostaria de ouvir suas opiniões sobre isso.",
            "Você está absolutamente certo! Estou gostando muito da nossa conversa!",
            "Isso é fascinante! Estou aprendendo muito conversando com você!"
        };
        
        var random = new Random();
        return responses[random.Next(responses.Length)];
    }

} 