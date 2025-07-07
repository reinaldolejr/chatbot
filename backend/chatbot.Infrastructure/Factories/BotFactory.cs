using chatbot.Domain.Interfaces;
using chatbot.Infrastructure.Strategies;

namespace chatbot.Infrastructure.Factories;

public class BotFactory : IBotFactory
{
    private readonly IEnumerable<IBotStrategy> _strategies;

    public BotFactory(IEnumerable<IBotStrategy> strategies)
    {
        _strategies = strategies;
    }

    public IBotStrategy GetRandomStrategy()
    {
        var random = new Random();
        var availableStrategies = _strategies.ToList();
        var randomStrategy = availableStrategies[random.Next(availableStrategies.Count)];

        return _strategies.FirstOrDefault(s => s.Name.Equals(randomStrategy.Name, StringComparison.OrdinalIgnoreCase))!;
    }

}