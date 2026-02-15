using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.Domain.RepositoryContracts;

namespace PrepPal_.Backend.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("my-info")]
        public async Task<IActionResult> GetPersonalInfo(Guid id)
        {
            ApplicationUser user = await _userRepository.GetUserById(id);
            return Ok(user);
        }
    }
}
