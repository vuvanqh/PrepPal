using System.Runtime.CompilerServices;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using PrepPal_.Core.Domain.RepositoryContracts;

namespace PrepPal_.Core;


/// <summary>
/// DO NOT USE - OBSOLETE
/// </summary>
public class CartInteractionHandler : IInteractionHandler
{
    private readonly IRecipeInteractionRepository _interactionRepository;
    public CartInteractionHandler(IRecipeInteractionRepository interactionRepository)
    {
        _interactionRepository = interactionRepository;
    }
    public InteractionType Type => InteractionType.AddToCart;

    public Task Handle(UserRecipeInteraction interaction, InteractionAction action)
    {
        //perhaps later it will require more work when using signalR
        return action switch{
            InteractionAction.Add => _interactionRepository.AddInteractionAsync(interaction),
            InteractionAction.Remove => _interactionRepository.RemoveInteractionAsync(interaction),
            _ => throw new InvalidOperationException()
        };
    }
}