using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public class RemoveConnectionCommand : IConnectionCommand
{
    public ConnectionAction Action => ConnectionAction.Remove;

    public Task Execute(Connection c, Guid actorId)
    {
        ConnectionGuards.EnsureAccepted(c);
        ConnectionGuards.EnsureParticipant(c, actorId);

        return Task.CompletedTask;
    }
}
