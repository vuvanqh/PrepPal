using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrepPal_.Core.Application.DTO.Account;
using PrepPal_.Core.ServiceContracts;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text.Json;

namespace PrepPal_.Backend.Controllers
{
    [Route("api/recipe-interaction")]
    [ApiController]
    public class RecipeInteractionController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        private readonly IRecipeInteractionService _recipeInteractionService;
        public RecipeInteractionController(IRecipeService recipeService, IRecipeInteractionService recipeInteractionService)
        {
            _recipeService = recipeService;
            _recipeInteractionService = recipeInteractionService;
        }

        [HttpGet("liked-recipes")]
        public async Task<IActionResult> GetLikedRecipes()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null)
                return NotFound();

            try
            {
                List<LikedRecipeResponse>? recipes = await _recipeInteractionService.GetLikedRecipes(Guid.Parse(id));
                return Ok(recipes);
            }
            catch (Exception e) {
                return NotFound(e.Message);
            }
        }

        [HttpPost("recipe-interaction")]
        public async Task<IActionResult> RecipeInteraction(UserRecipeInteractionRequest request)
        {
            try
            {
                await _recipeInteractionService.Interact( Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!), request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    ex.Message,
                    Inner = ex.InnerException?.Message,
                    Type = ex.GetType().FullName
                });
            }
        }
    }
}
