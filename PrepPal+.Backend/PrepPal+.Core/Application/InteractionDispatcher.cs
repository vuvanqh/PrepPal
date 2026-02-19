using Microsoft.VisualBasic;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using PrepPal_.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public class InteractionDispatcher
{
    private IReadOnlyDictionary<InteractionType, IInteractionHandler> _handlers;
    public InteractionDispatcher(IEnumerable<IInteractionHandler> handlers)
    {
        _handlers = handlers.ToDictionary(h => h.Type);
    }

    public Task Dispatch(UserRecipeInteraction interaction, InteractionAction action)
    {
        if (!_handlers.TryGetValue(interaction.Type, out IInteractionHandler? handler))
            throw new InvalidOperationException($"No handler for {interaction.Type}");

        return handler.Handle(interaction, action);
    }
}
