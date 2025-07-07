using chatbot.Domain.Entities;
using chatbot.Domain.Exceptions;
using chatbot.Domain.Interfaces;
using MediatR;

namespace chatbot.Application.Commands.AddMessage;

public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand, AddMessageResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISubject _notificationSubject;
    private readonly IBotService _botService;

    public AddMessageCommandHandler(IUnitOfWork unitOfWork, ISubject notificationSubject, IBotService botService)
    {
        _unitOfWork = unitOfWork;
        _notificationSubject = notificationSubject;
        _botService = botService;
    }

    public async Task<AddMessageResponse> Handle(AddMessageCommand request, CancellationToken cancellationToken)
    {
        // Verificar se o chat existe
        var chat = await _unitOfWork.Chats.GetByIdAsync(request.ChatId);
        if (chat == null)
        {
            throw NotFoundException.Create<Chat>(request.ChatId.ToString());
        }

        // Verificar se o chat foi encerrado
        if (chat.EndedAt.HasValue)
        {
            throw new InvalidOperationException("Não é possível adicionar mensagem a um chat encerrado");
        }


        await _botService.GetBotResponseAsync(request.ChatId, request.Content, request.Sender);


        return new AddMessageResponse
        {
            ChatId = request.ChatId,
            Sender = request.Sender,
            Content = request.Content,
            Timestamp = DateTime.UtcNow
        };
    }
}