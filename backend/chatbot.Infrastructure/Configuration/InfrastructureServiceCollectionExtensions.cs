using chatbot.Domain.Interfaces;
using chatbot.Infrastructure.Data;
using chatbot.Infrastructure.Repositories;
using chatbot.Infrastructure.UnitOfWork;
using chatbot.Infrastructure.Observers;
using chatbot.Infrastructure.Strategies;
using chatbot.Infrastructure.Services;
using chatbot.Application.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using chatbot.Infrastructure.Factories;

namespace chatbot.Infrastructure.Configuration;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddApplicationServices();

        // Add DbContext
        services.AddDbContext<ChatbotDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("chatbot.Infrastructure")));

        // Register repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWorkImplementation>();

        // Register notification services
        services.AddSingleton<ISubject, NotificationSubject>();
        services.AddScoped<INotificationService, NotificationService>();

        // Register bot strategies
        services.AddScoped<IBotStrategy, FriendlyBotStrategy>();
        services.AddScoped<IBotStrategy, ProfessionalBotStrategy>();

        // Register bot service
        services.AddScoped<IBotService, BotService>();

        // Register bot factory
        services.AddScoped<IBotFactory, BotFactory>();
        

        return services;
    }
} 