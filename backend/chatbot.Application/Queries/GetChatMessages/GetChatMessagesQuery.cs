using MediatR;

namespace chatbot.Application.Queries.GetChatMessages;

public class GetChatMessagesQuery : IRequest<GetChatMessagesResponse>
{
    public Guid ChatId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
} 