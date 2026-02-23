using PrepPal_.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepPal_.Core.DTO;

public class ConnectionResponse
{
    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public string LastName { get; set; } = string.Empty;
    [Required] public string UserName { get; set; } = string.Empty;
    [Required] public Guid ConnectionId { get; set; }
    [Required] public string RequestedByUsername { get; set; }= string.Empty;
    [Required] public Status Status { get; set; }
}

public static class ConnectionExtention
{
    public static ConnectionResponse ToConnectionResponse(this Connection c, Guid actorId)
    {
        ApplicationUser user = c.UserId1 == actorId ? c.User2 : c.User1;
        return new ConnectionResponse()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName!,
            ConnectionId = c.Id,
            RequestedByUsername = c.RequestedByUserId == c.UserId1 ? c.User1.UserName! : c.User2.UserName!,
            Status = c.Status,
        };
    }
}