using chatbot.Application.Commands.AddMessage;
using chatbot.Application.Commands.CreateChat;
using chatbot.Application.Queries.GetChatMessages;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace chatbot.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Cria um novo chat
    /// </summary>
    /// <param name="request">Detalhes da criação do chat</param>
    /// <returns>Informações do chat criado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateChatResponse), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<CreateChatResponse>> CreateChat([FromBody] CreateChatRequest request)
    {
        var command = new CreateChatCommand
        {
            Name = request.Name
        };

        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetChatMessages), new { chatId = result.Id }, result);
    }

    /// <summary>
    /// Adiciona uma mensagem a um chat específico
    /// </summary>
    /// <param name="chatId">O ID do chat</param>
    /// <param name="request">Detalhes da mensagem</param>
    /// <returns>Created message information</returns>
    [HttpPost("{chatId}/message")]
    [ProducesResponseType(typeof(AddMessageResponse), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<AddMessageResponse>> AddMessage(
        Guid chatId, 
        [FromBody] AddMessageRequest request)
    {
        var command = new AddMessageCommand
        {
            ChatId = chatId,
            Sender = request.Sender,
            Content = request.Content
        };

        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetChatMessages), new { chatId = result.ChatId }, result);
    }

    /// <summary>
    /// Retorna mensagens para um chat específico com paginação
    /// </summary>
    /// <param name="chatId">O ID do chat</param>
    /// <param name="page">Número da página (padrão: 1)</param>
    /// <param name="pageSize">Tamanho da página (padrão: 50, máx: 100)</param>
    /// <returns>Mensagens do chat com informações de paginação</returns>
    [HttpGet("{chatId}/messages")]
    [ProducesResponseType(typeof(GetChatMessagesResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<GetChatMessagesResponse>> GetChatMessages(
        Guid chatId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        var query = new GetChatMessagesQuery
        {
            ChatId = chatId,
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }
} 