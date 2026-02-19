using System.Runtime.CompilerServices;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using PrepPal_.Core.Domain.RepositoryContracts;

namespace PrepPal_.Core;

public class LikeInteractionHandler : IInteractionHandler
{
    private readonly IRecipeInteractionRepository _interactionRepository;
    public LikeInteractionHandler(IRecipeInteractionRepository interactionRepository)
    {
        _interactionRepository = interactionRepository;
    }
    public InteractionType Type => InteractionType.Like;

    public Task Handle(UserRecipeInteraction interaction, InteractionAction action)
    {
        return action switch{
            InteractionAction.Add => _interactionRepository.AddInteractionAsync(interaction),
            InteractionAction.Remove => _interactionRepository.RemoveInteractionAsync(interaction),
            _ => throw new InvalidOperationException()
        };
    }
}