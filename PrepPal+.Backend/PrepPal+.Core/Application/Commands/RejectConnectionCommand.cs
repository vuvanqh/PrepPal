using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public class RejectConnectionCommand: IConnectionCommand
{
    public ConnectionAction Action => ConnectionAction.Reject;

    public Task Execute(Connection c, Guid actorId)
    {
        ConnectionGuards.EnsurePending(c);
        ConnectionGuards.EnsureParticipant(c, actorId);
        ConnectionGuards.EnsureReceiver(c, actorId);

        return Task.CompletedTask;
    }

}
