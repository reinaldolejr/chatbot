using chatbot.Domain.Interfaces;
using chatbot.Infrastructure.Observers;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace chatbot.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WebSocketController : ControllerBase
{
    private readonly ISubject _notificationSubject;
    private readonly INotificationService _notificationService;

    public WebSocketController(ISubject notificationSubject, INotificationService notificationService)
    {
        _notificationSubject = notificationSubject;
        _notificationService = notificationService;
    }

    [HttpGet("ws/notifications/{chatId}")]
    public async Task GetChatNotifications(Guid chatId)
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var connectionId = Guid.NewGuid().ToString();

            // Registrar observer para o chat
            IObserver observer = await _notificationService.RegisterObserverAsync(chatId.ToString(), connectionId);
            WebSocketObserver? wsObserver = observer as WebSocketObserver;

            if (wsObserver == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return;
            }
            
            var sendTask = Task.Run(async () =>
            {
                await foreach (var message in wsObserver.ChannelReader.ReadAllAsync())
                {
                    var bytes = Encoding.UTF8.GetBytes(message);
                    await webSocket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            });

            try
            {
                await HandleWebSocketConnection(webSocket, observer, connectionId, chatId);
            }
            finally
            {
                // Unregister observer quando a conexão é fechada
                await _notificationService.UnregisterObserverAsync(connectionId);
                wsObserver?.Complete();
                await sendTask;
            }
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private async Task HandleWebSocketConnection(WebSocket webSocket, IObserver observer, string connectionId, Guid? chatId)
    {
        var buffer = new byte[1024 * 4];


        while (webSocket.State == WebSocketState.Open)
        {
            try
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by client", CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                var errorMessage = new
                {
                    Type = "Error",
                    Message = "WebSocket error occurred",
                    Error = ex.Message,
                    ChatId = chatId,
                    Timestamp = DateTime.UtcNow
                };

                var errorJson = JsonSerializer.Serialize(errorMessage);
                var errorBytes = Encoding.UTF8.GetBytes(errorJson);

                await webSocket.SendAsync(errorBytes, WebSocketMessageType.Text, true, CancellationToken.None);
                await webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Internal server error", CancellationToken.None);

                break;
            }
        }
    }
}