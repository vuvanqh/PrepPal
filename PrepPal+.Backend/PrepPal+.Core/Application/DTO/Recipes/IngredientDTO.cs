using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepPal_.Core.Application.DTO;

public class IngredientDTO
{
    [Required] public string IngredientName { get; set; } = null!;
    [Required] public string IngredientMeasure { get; set; } = null!;
}
