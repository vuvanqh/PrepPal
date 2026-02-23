using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PrepPal_.Backend.Hubs;
using PrepPal_.Core;
using PrepPal_.Core.Application.DTO;
using System.Security.Claims;

namespace PrepPal_.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;
    private readonly IMessageService _messageService;
    private readonly IConnectionService _connectionService;
    public ChatController(IHubContext<ChatHub, IChatClient> hubContext, IMessageService messgeService, IConnectionService connectionService )
    {
        _hubContext = hubContext;
        _messageService = messgeService;
        _connectionService = connectionService;
    }

    [HttpPost("send/{connectionId:guid}")]
    public async Task<IActionResult> SendMessage(Guid connectionId, [FromBody] SendMessageRequest request)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return Unauthorized();
        try
        {
            MessageResponse message = await _messageService.SendMessage(Guid.Parse(id), connectionId, request.Message);
            Guid receiverId = await _connectionService.GetReceiverId(Guid.Parse(id), connectionId);

            await _hubContext.Clients.User(receiverId.ToString()).ReceiveMessage(message, connectionId);
            await _hubContext.Clients.User(id.ToString()).ReceiveMessage(message, connectionId);
            return Ok(message);
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return BadRequest(ex.Message);
        }
    }


    [HttpGet("get-conversation/{connectionId:guid}")]
    public async Task<IActionResult> GetConversation(Guid connectionId)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return Unauthorized();
        try
        {
            ConversationResponse resp = await _messageService.GetConversation(Guid.Parse(id), connectionId);
            return Ok(resp);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}


