namespace chatbot.Application.Commands.AddMessage;

public class AddMessageRequest
{
    public string Sender { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
