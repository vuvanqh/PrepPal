using Microsoft.AspNetCore.Identity;
using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.DTO.Account;
using PrepPal_.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Text;
using PrepPal_.Core.Errors; 

namespace PrepPal_.Core.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser> Register(RegisterRequest? registerRequest)
    {
        ApplicationUser user = registerRequest!.ToUser();

        var result = await _userManager.CreateAsync(user, registerRequest.Password);
        if (!result.Succeeded)
        {
            throw new IdentityOperationException(result.Errors.Select(e => e.Description));
        }
        //await _userManager.AddToRoleAsync(user, UserRoles.User);
        return user;
    }
}
