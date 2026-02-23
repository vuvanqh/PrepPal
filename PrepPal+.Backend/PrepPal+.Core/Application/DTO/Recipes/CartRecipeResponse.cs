using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepPal_.Core.Application.DTO.Recipes;

public class CartRecipeResponse
{
    [Required]  public RecipeResponse Recipe { get; set; } = null!;
    [Required]  public int Quantity { get; set; }
}
