namespace chatbot.Domain.Interfaces;

public interface IBotFactory
{
    IBotStrategy GetRandomStrategy();
}
