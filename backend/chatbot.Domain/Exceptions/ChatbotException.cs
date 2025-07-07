namespace chatbot.Domain.Exceptions;

public abstract class ChatbotException : Exception
{
    public int StatusCode { get; }

    protected ChatbotException(string message, int statusCode = 500) : base(message)
    {
        StatusCode = statusCode;
    }

    protected ChatbotException(string message, Exception innerException, int statusCode = 500) 
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }
} 