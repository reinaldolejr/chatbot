using chatbot.Domain.Entities;
using chatbot.Domain.Exceptions;
using chatbot.Domain.Interfaces;
using MediatR;

namespace chatbot.Application.Queries.GetChatMessages;

public class GetChatMessagesQueryHandler : IRequestHandler<GetChatMessagesQuery, GetChatMessagesResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetChatMessagesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GetChatMessagesResponse> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
    {
        // Verifica se o chat existe
        var chat = await _unitOfWork.Chats.GetByIdAsync(request.ChatId);
        if (chat == null)
        {
            throw NotFoundException.Create<Chat>(request.ChatId.ToString());
        }

        // Retorna as mensagens para o chat
        var messages = await _unitOfWork.Messages.GetMessagesByChatIdAsync(request.ChatId);
        var messagesList = messages.ToList();

        // Aplica a paginação
        var totalCount = messagesList.Count;
        var skip = (request.Page - 1) * request.PageSize;
        var paginatedMessages = messagesList
            .Skip(skip)
            .Take(request.PageSize)
            .Select(m => new MessageDto
            {
                Id = m.Id,
                ChatId = m.ChatId,
                Sender = m.Sender,
                Content = m.Content,
                Timestamp = m.Timestamp
            })
            .ToList();

        return new GetChatMessagesResponse
        {
            ChatId = request.ChatId,
            Name = chat.Name,
            Messages = paginatedMessages,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}