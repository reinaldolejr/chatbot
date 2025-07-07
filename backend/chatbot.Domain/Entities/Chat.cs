namespace chatbot.Domain.Entities;

public class Chat
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
