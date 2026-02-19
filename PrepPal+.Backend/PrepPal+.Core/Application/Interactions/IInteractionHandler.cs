using PrepPal_.Core.Domain.Entities.RecipeEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public interface IInteractionHandler
{
    InteractionType Type { get; }
    Task Handle(UserRecipeInteraction interaction, InteractionAction action);
}
