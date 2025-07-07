using MediatR;

namespace chatbot.Application.Commands.AddMessage;

public class AddMessageCommand : IRequest<AddMessageResponse>
{
    public Guid ChatId { get; set; }
    public string Sender { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
} 