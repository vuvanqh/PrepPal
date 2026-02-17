using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrepPal_.Core.Application.DTO;
using PrepPal_.Core.Application.DTO.Account;
using PrepPal_.Core.Application.ServiceContracts;
using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Core.DTO.Account;
using PrepPal_.Core.ServiceContracts;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text.Json;

namespace PrepPal_.Backend.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRecipeService _recipeService;
        private readonly IAccountService _accountService;
        public AccountController(IUserRepository userRepository, IRecipeService recipeService, IAccountService accountService)
        {
            _userRepository = userRepository;
            _recipeService = recipeService;
            _accountService = accountService;
        }

        [HttpGet("my-info")]
        public async Task<IActionResult> GetPersonalInfo()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(id==null)
                return NotFound();
            try
            {
                PersonalDetailsResponse? userDetails = await _accountService.GetPersonalDetails(Guid.Parse(id));
                return Ok(userDetails);
            }
            catch(Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("liked-recipes")]
        public async Task<IActionResult> GetLikedRecipes()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null)
                return NotFound();

            try
            {
                List<RecipeResponse>? recipes = await _accountService.GetLikedRecipes(Guid.Parse(id));
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
                await _recipeService.Interact(request, Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!));
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
