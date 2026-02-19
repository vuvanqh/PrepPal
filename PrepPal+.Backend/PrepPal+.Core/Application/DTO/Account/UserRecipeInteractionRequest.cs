using PrepPal_.Core.Domain.Entities.RecipeEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepPal_.Core.Application.DTO.Account;

public class UserRecipeInteractionRequest
{
    [Required] public int ExternalRecipeId { get; set; }
    [Required] public InteractionType Type { get; set; }
    [Required] public InteractionAction Action {get;set;}
}
