using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PrepPal_.Backend.Hubs;
using PrepPal_.Core;
using PrepPal_.Core.Application.DTO.Social;
using PrepPal_.Core.DTO;
using PrepPal_.Core.ServiceContracts;
using System.Security.Claims;

namespace PrepPal_.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConnectionController : ControllerBase
{
    private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
    private readonly IConnectionService _connectionService;
    public ConnectionController(IHubContext<NotificationHub, INotificationClient> hubContext, IConnectionService connectionService)
    {
        _hubContext = hubContext;
        _connectionService = connectionService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetConnections()
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return Unauthorized();

        List<ConnectionResponse> resp = await _connectionService.GetAllFriends(Guid.Parse(id));
        return Ok(resp);
    }

    [HttpPost("request")]
    public async Task<IActionResult> RequestConnection(MakeConnectionRequest request)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return Unauthorized();

        ConnectionResponse resp = await _connectionService.AddConnectionRequest(Guid.Parse(id), request.UserName);
        Guid receiverId = await _connectionService.GetReceiverId(Guid.Parse(id), resp.ConnectionId);

        await _hubContext.Clients.User(receiverId.ToString()).ReceiveConnectionRequestNotification(User.Identity?.Name!);
        Console.WriteLine(User.Identity?.Name!);
        return Ok(resp);
    }
    [HttpPatch("modify-connection")]
    public async Task<IActionResult> ModifyConnection(ConnectionActionRequest request)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return Unauthorized("unauthorized access");

        await _connectionService.ModifyConnection(Guid.Parse(id), request.ConnectionId, request.Action);

        if (request.Action == ActionType.Accept)
        {
            Guid receiverId = await _connectionService.GetReceiverId(Guid.Parse(id), request.ConnectionId);

            //switch to events later
            await _hubContext.Clients.User(receiverId.ToString()).NotifyConnectionAccepted(User.Identity?.Name!);
        }

        return Ok();
    }
    [HttpGet("search/{search}")]
    public async Task<IActionResult> SearchForUsers(string search)
    {
        List<UserReposnse> resp = await _connectionService.SearchByUser(search);
        return Ok(resp.Where(ur => ur.UserName != User.Identity?.Name).ToList());

    }
}
