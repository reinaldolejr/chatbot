using chatbot.Domain.Entities;
using chatbot.Domain.Interfaces;

namespace chatbot.Infrastructure.Services;

public class BotService : IBotService
{
    private readonly IEnumerable<IBotStrategy> _strategies;
    private readonly IRepository<Chat> _chatRepository;
    private readonly IRepository<Message> _messageRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;
    private readonly IBotFactory _botFactory;
    public BotService(
        IEnumerable<IBotStrategy> strategies,
        IRepository<Chat> chatRepository,
        IRepository<Message> messageRepository,
        IUnitOfWork unitOfWork,
        INotificationService notificationService,
        ISubject notificationSubject,
        IBotFactory botFactory)
    {
        _strategies = strategies;
        _chatRepository = chatRepository;
        _messageRepository = messageRepository;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
        _botFactory = botFactory;
    }

    public async Task<string> GetBotResponseAsync(Guid chatId, string userMessage, string sender)
    {
        IBotStrategy strategy = _botFactory.GetRandomStrategy();
      
        var chat = await _chatRepository.GetByIdAsync(chatId);
        if (chat == null)
        {
            chat = new Chat
            {
                Id = chatId,
                Name = $"Chat with {sender}",
                StartedAt = DateTime.UtcNow
            };
            await _chatRepository.AddAsync(chat);
        }

        // salvar mensagem do usuário
        var userMessageEntity = new Message
        {
            Id = Guid.NewGuid(),
            ChatId = chatId,
            Content = userMessage,
            Sender = sender,
            Timestamp = DateTime.UtcNow
        };
        await _messageRepository.AddAsync(userMessageEntity);

        // obter histórico da conversa
        var conversationHistory = await _messageRepository.GetAllAsync();
        var chatHistory = conversationHistory.Where(m => m.ChatId == chatId).OrderBy(m => m.Timestamp).ToList();

        // gerar resposta do bot
        var botResponse = await strategy.GenerateResponseAsync(userMessageEntity, chatHistory);

        // salvar resposta do bot
        var botMessageEntity = new Message
        {
            Id = Guid.NewGuid(),
            ChatId = chatId,
            Content = botResponse,
            Sender = "ChatBot",
            Timestamp = DateTime.UtcNow
        };
        await _messageRepository.AddAsync(botMessageEntity);

        // finalizar o chat quando o usuário escrever "sair"
        if (userMessage == "sair")
        {
            chat.EndedAt = DateTime.UtcNow;
            await _chatRepository.UpdateAsync(chat);
        }

        await _unitOfWork.SaveChangesAsync();

        // envia mensagem para o usuário através do serviço de notificação
        var notificationMessage = new NotificationMessage
        {
            Type = "BotMessage",
            Title = "Bot Response",
            Content = botResponse,
            Sender = "chatbot",
            ReceiverId = chatId.ToString(),
            Metadata = new Dictionary<string, object>
            {
                { "ChatId", chatId },
                { "MessageId", botMessageEntity.Id },
                { "Strategy", strategy.Name },
                { "Response", botResponse }
            }
        };
        await _notificationService.SendNotificationAsync(notificationMessage);

        return botResponse;
    }


}