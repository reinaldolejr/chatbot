using System.Collections.Generic;

namespace chatbot.Domain.Exceptions;

public class ValidationException : ChatbotException
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationException(string message) : base(message, 400)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(string message, Dictionary<string, string[]> errors) : base(message, 400)
    {
        Errors = errors;
    }

    public ValidationException(string message, Exception innerException) : base(message, innerException, 400)
    {
        Errors = new Dictionary<string, string[]>();
    }
} 