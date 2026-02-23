using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public static class ConnectionGuards
{
    public static void EnsureParticipant(Connection c, Guid actorId)
    {
        if (c.UserId1 != actorId && c.UserId2 != actorId)
            throw new InvalidOperationException("Not a participant");
    }

    public static void EnsurePending(Connection c)
    {
        if (c.Status != Status.Pending)
            throw new InvalidOperationException("Not pending");
    }

    public static void EnsureAccepted(Connection c)
    {
        if (c.Status != Status.Accepted)
            throw new InvalidOperationException("Not accepted");
    }

    public static void EnsureRequester(Connection c, Guid actorId)
    {
        if (c.RequestedByUserId != actorId)
            throw new InvalidOperationException("Not requester");
    }

    public static void EnsureReceiver(Connection c, Guid actorId)
    {
        if (c.RequestedByUserId == actorId)
            throw new InvalidOperationException("Not receiver");
    }
}