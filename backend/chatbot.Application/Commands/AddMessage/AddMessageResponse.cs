namespace chatbot.Application.Commands.AddMessage;

public class AddMessageResponse
{
    public Guid ChatId { get; set; }
    public string Sender { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
