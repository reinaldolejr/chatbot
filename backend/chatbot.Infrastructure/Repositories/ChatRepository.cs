using chatbot.Domain.Entities;
using chatbot.Domain.Interfaces;
using chatbot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace chatbot.Infrastructure.Repositories;

public class ChatRepository : Repository<Chat>, IChatRepository
{
    public ChatRepository(ChatbotDbContext context) : base(context)
    {
    }

} 