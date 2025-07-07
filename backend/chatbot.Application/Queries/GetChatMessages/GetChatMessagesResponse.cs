namespace chatbot.Application.Queries.GetChatMessages;

public class GetChatMessagesResponse
{
    public Guid ChatId { get; set; }
    public string Name { get; set; } = string.Empty;
    public IEnumerable<MessageDto> Messages { get; set; } = new List<MessageDto>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}


public class MessageDto
{
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public string Sender { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
