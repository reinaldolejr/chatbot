using chatbot.Domain.Entities;
using chatbot.Domain.Interfaces;

namespace chatbot.Infrastructure.Strategies;

public class ProfessionalBotStrategy : IBotStrategy
{
    public string Name => "Profissional";

    public async Task<string> GenerateResponseAsync(Message userMessage, IEnumerable<Message> conversationHistory)
    {
        var userInput = userMessage.Content.ToLower();
        
        // respostas baseadas em palavras-chave
        if (userInput.Contains("olá") || userInput.Contains("oi") || userInput.Contains("eai"))
        {
            return "Olá. Estou aqui para ajudá-lo com qualquer consulta que você possa ter. Como posso ajudá-lo hoje?";
        }
        
        if (userInput.Contains("como você está"))
        {
            return "Estou funcionando perfeitamente, obrigado por perguntar. Como posso ajudar você hoje?";
        }
        
        if (userInput.Contains("ajuda") || userInput.Contains("suporte"))
        {
            return "Estou disponível para fornecer assistência. Por favor, especifique a natureza de sua consulta para que eu possa melhor atendê-lo.";
        }
        
        if (userInput.Contains("obrigado"))
        {
            return "De nada. Estou aqui para ajudar. Há algo mais que você precisa?";
        }
        
        if (userInput.Contains("reunião") || userInput.Contains("agendar"))
        {
            return "Posso ajudar com agendamentos. Por favor, forneça os detalhes de suas necessidades de reunião.";
        }
        
        if (userInput.Contains("relatório") || userInput.Contains("dados"))
        {
            return "Posso ajudar com análise de dados e geração de relatórios. Qual informação específica você está procurando?";
        }
        
        if (userInput.Contains("sair"))
        {
            return "Adeus. Obrigado por seu tempo. Tenha um dia produtivo.";
        }
        
        // respostas padrão se não houver resposta para a mensagem do usuário
        var responses = new[]
        {
            "Entendo sua consulta. Vou fornecer as informações relevantes.",
            "Obrigado por sua mensagem. Estou processando sua solicitação e responderei conforme necessário.",
            "Agradeço sua entrada. Como gostaria de proceder com esta questão?",
            "Sua mensagem foi recebida. Estou aqui para ajudar com quaisquer outras perguntas.",
            "Agradeço sua comunicação. Por favor, me informe se você precisa de suporte adicional."
        };
        
        var random = new Random();
        return responses[random.Next(responses.Length)];
    }

} 