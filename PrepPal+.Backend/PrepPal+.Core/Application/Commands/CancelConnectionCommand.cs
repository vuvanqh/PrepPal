using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public class CancelConnectionCommand: IConnectionCommand
{
    public ActionType Action => ActionType.Cancel;

    public Task Execute(Connection c, Guid actorId)
    {
        ConnectionGuards.EnsurePending(c);
        ConnectionGuards.EnsureParticipant(c, actorId);
        ConnectionGuards.EnsureRequester(c, actorId);

        return Task.CompletedTask;
    }
}

