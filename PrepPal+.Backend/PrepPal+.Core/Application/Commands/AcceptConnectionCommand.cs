using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public class AcceptConnectionCommand: IConnectionCommand
{
    public ActionType Action => ActionType.Accept;

    public Task Execute(Connection c, Guid actorId)
    {
        ConnectionGuards.EnsurePending(c);
        ConnectionGuards.EnsureParticipant(c, actorId);
        ConnectionGuards.EnsureReceiver(c, actorId);

        c.Status = Status.Accepted;
        return Task.CompletedTask;
    }
}
