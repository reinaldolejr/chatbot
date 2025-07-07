using chatbot.Domain.Entities;
using chatbot.Domain.Interfaces;
using MediatR;

namespace chatbot.Application.Commands.CreateChat;

public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, CreateChatResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateChatCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateChatResponse> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var chat = new Chat
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            StartedAt = DateTime.UtcNow
        };

        await _unitOfWork.Chats.AddAsync(chat);
        await _unitOfWork.SaveChangesAsync();

        return new CreateChatResponse
        {
            Id = chat.Id,
            Name = chat.Name,
            StartedAt = chat.StartedAt,
            EndedAt = chat.EndedAt,
            MessageCount = 0
        };
    }
} 