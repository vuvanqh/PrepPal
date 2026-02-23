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
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;
        public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Passwords don't match")]
        [ProducesResponseType(StatusCodes.Status409Conflict, Description = "Email already exists")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            //_logger.LogInformation("/account/register");
            //_logger.LogDebug($"{registerRequest.FirstName} {registerRequest.LastName} - {registerRequest.UserName}");
            try
            {
                ApplicationUser user = await _authenticationService.Register(registerRequest);
                return Ok();
            }
            catch (IdentityOperationException ex)
            {
                _logger.LogError(ex.Message);
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
            //_logger.LogInformation("/account/login");
            //_logger.LogDebug($"{loginRequest.Email}");      
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return Ok(new { Message = "User is already signed in.", User = User.Identity.Name });

            try
            {
                (LoginResponse response, RefreshTokenResult refreshTokenResult) = await _authenticationService.Login(loginRequest);
                Response.Cookies.Append("refreshToken", refreshTokenResult.RefreshToken, new CookieOptions()
                {
                    HttpOnly = true, //prevents js/ts from reading the cookie & protects against xss attacks
                    Secure = true, //only sent over https
                    SameSite = SameSiteMode.Lax, //blocks csrf attacks
                    Expires = refreshTokenResult.ExpirationDate,
                    Path = "/api/auth"
                });
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("refreshToken")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Description = "Token expired")]
        public async Task<IActionResult> RefreshToken()
        {
            string? token  = Request.Cookies["refreshToken"];
            _logger.LogInformation(token);
            if (token != null)
            {
                try
                {
                    RefreshTokenResponse resp = await _authenticationService.RotateRefreshToken(token);

                    Response.Cookies.Append("refreshToken", resp.RefreshToken, new CookieOptions()
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Lax,
                        Expires = resp.ExpirationDate,
                        Path = "/api/auth"
                    });

                    return Ok(resp.AccessToken);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Unauthorized(ex.Message);
                }
            }
  
             return Unauthorized("Session Expired");
            
        }
    }
}
