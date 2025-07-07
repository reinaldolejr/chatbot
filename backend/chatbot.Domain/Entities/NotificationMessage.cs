namespace chatbot.Domain.Entities;

public class NotificationMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Sender { get; set; }
    public string? ReceiverId { get; set; }
    public DateTime Timestamp { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }

    public NotificationMessage()
    {
        Id = Guid.NewGuid();
        Timestamp = DateTime.UtcNow;
        Metadata = new Dictionary<string, object>();
    }
}
