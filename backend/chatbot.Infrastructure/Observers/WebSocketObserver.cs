using System.Text.Json;
using System.Threading.Channels;
using System.Net.WebSockets;
using System.Text;
using chatbot.Domain.Interfaces;
using chatbot.Domain.Entities;

namespace chatbot.Infrastructure.Observers;

public class WebSocketObserver : IObserver
{
    private readonly Channel<string> _channel;
    public string Id => ConnectionId;
    public string ConnectionId { get; }
    public string ChatId { get; }
    public bool IsActive { get; private set; } = true;
    public DateTime LastActivity { get; private set; } = DateTime.UtcNow;

    public WebSocketObserver(string chatId, string connectionId)
    {
        ChatId = chatId;
        ConnectionId = connectionId;
        _channel = Channel.CreateUnbounded<string>();
    }

    public ChannelReader<string> ChannelReader => _channel.Reader;

    public Task NotifyAsync(NotificationMessage message)
    {
        LastActivity = DateTime.UtcNow;

        var options = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        var json = JsonSerializer.Serialize(new
        {
            Type = "NewMessage",
            Data = message,
            ChatId = ChatId,
            Timestamp = DateTime.UtcNow
        }, options);
        return _channel.Writer.WriteAsync(json).AsTask();
    }

    public void Complete()
    {
        IsActive = false;
        _channel.Writer.TryComplete();
    }
}