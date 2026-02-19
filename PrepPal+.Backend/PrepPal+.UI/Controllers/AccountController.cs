using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrepPal_.Core.Application.DTO;
using PrepPal_.Core.Application.DTO.Account;
using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Core.DTO.Account;
using PrepPal_.Core.ServiceContracts;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text.Json;

namespace PrepPal_.Backend.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    public AccountController( IAccountService accountService)
    {
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
}
