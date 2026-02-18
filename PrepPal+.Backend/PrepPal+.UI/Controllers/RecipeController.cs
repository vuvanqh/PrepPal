using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrepPal_.Core.Application.DTO;
using PrepPal_.Core.Application.ServiceContracts;

namespace PrepPal_.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _recipeService;

    public RecipeController(IRecipeService recipeService) {  
        _recipeService = recipeService; 
    }

    [AllowAnonymous]
    [HttpGet("random")]
    public async Task<IActionResult> Get10RandomRecipes()
    {
        var response = await _recipeService.Get10RandomRecipes();
        return Ok(response);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchRecipesByName(string name)
    {
        List<RecipeResponse> resp = await _recipeService.SearchRecipesByName(name)?? new List<RecipeResponse>();

        return Ok(new {recipes =  resp});
    }
}
