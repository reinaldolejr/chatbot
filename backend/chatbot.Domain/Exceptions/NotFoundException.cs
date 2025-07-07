namespace chatbot.Domain.Exceptions;

public class NotFoundException : ChatbotException
{
    public NotFoundException(string message) : base(message, 404)
    {
    }

    public NotFoundException(string message, Exception innerException) : base(message, innerException, 404)
    {
    }

    public static NotFoundException Create<T>(string id) where T : class
    {
        return new NotFoundException($"{typeof(T).Name} with id '{id}' was not found.");
    }

    public static NotFoundException Create<T>(string property, string value) where T : class
    {
        return new NotFoundException($"{typeof(T).Name} with {property} '{value}' was not found.");
    }
} 