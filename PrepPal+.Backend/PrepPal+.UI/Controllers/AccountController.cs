using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrepPal_.Core.Application.DTO.Account;
using PrepPal_.Core.Application.ServiceContracts;
using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.Domain.RepositoryContracts;
using System.Security.Claims;

namespace PrepPal_.Backend.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRecipeService _recipeService;
        public AccountController(IUserRepository userRepository, IRecipeService recipeService)
        {
            _userRepository = userRepository;
            _recipeService = recipeService;
        }

        [HttpGet("my-info")]
        public async Task<IActionResult> GetPersonalInfo(Guid id)
        {
            ApplicationUser user = await _userRepository.GetUserById(id);
            return Ok(user);
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
