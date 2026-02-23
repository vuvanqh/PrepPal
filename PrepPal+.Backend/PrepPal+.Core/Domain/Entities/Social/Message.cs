using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public class Message
{
    public required Guid Id { get; set; }
    public DateTime TimeStamp { get; set; }
    public string SenderUsername { get; set; } = null!;
    public string Content { get; set; } = null!;
    public required Guid ConnectionId { get; set; }

    public Connection Connection { get; set; } = null!;
}
