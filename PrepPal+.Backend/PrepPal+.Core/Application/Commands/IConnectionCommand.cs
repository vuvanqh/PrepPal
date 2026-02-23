using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public interface IConnectionCommand
{
    public ConnectionAction Action { get; }
    Task Execute(Connection c, Guid actorId);
}
