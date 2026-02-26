using PrepPal_.Core.Domain.Entities.RecipeEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public class ConnectionCommandDispatcher
{
    private IReadOnlyDictionary<ActionType, IConnectionCommand> _handlers;
    public ConnectionCommandDispatcher(IEnumerable<IConnectionCommand> handlers)
    {
        _handlers = handlers.ToDictionary(h => h.Action);
    }

    public Task Dispatch(Connection connection, Guid actorId , ActionType action)
    {
        if (!_handlers.TryGetValue(action, out IConnectionCommand? handler))
            throw new InvalidOperationException($"No handler for {action}");

        return handler.Execute(connection, actorId);
    }
}
