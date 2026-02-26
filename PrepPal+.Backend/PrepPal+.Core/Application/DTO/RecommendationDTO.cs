using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public record RecommendationRequest
{
    public List<Guid> likes { get; set; } = new List<Guid>();
    public List<RecommendationRecipeReq> recipes { get; set; } = new List<RecommendationRecipeReq>();
}


public record RecommendationRecipeReq
{
    public Guid recipeId { get; set; }
    public string category { get; set; } = null!;
    public string area { get; set; } = null!;
    public List<string> ingredients { get; set; } = new List<string>();
}
