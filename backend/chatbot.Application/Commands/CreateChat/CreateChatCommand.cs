using MediatR;

namespace chatbot.Application.Commands.CreateChat;

public class CreateChatCommand : IRequest<CreateChatResponse>
{
    public string Name { get; set; } = string.Empty;
} 