using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.DTO.Account;
using PrepPal_.Core.Errors;
using PrepPal_.Core.ServiceContracts;

namespace PrepPal_.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountService _accountService;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IAccountService accountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Passwords don't match")]
        [ProducesResponseType(StatusCodes.Status409Conflict, Description = "Email already exists")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            try
            {
                ApplicationUser user = await _accountService.Register(registerRequest);
                return Ok(user);
            }
            catch (IdentityOperationException ex)
            {
                return BadRequest(new
                {
                    errors = ex.Errors
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Description = "Invalid credentials")]
        public async Task<IActionResult> LogIn(LoginRequest loginRequest)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return Ok(new { Message = "User is already signed in.", User = User.Identity.Name });

            var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, isPersistent: false, lockoutOnFailure: true);
            return result.Succeeded? Ok(new LoginResponse((await _userManager.FindByEmailAsync(loginRequest.Email))!)) : Unauthorized();
        }
    }
}
