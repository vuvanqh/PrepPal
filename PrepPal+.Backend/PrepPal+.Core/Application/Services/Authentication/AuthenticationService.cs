using Microsoft.AspNetCore.Identity;
using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Core.DTO.Account;
using PrepPal_.Core.Errors;
using PrepPal_.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PrepPal_.Core.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenService _refreshTokenService;

    public AuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtService jwtService,
        IUserRepository userRepository, IRefreshTokenService refreshTokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _userRepository = userRepository;
        _refreshTokenService = refreshTokenService;
    }

    public async Task<(LoginResponse, RefreshTokenResult)> Login(LoginRequest? loginRequest) //NOT HERE
    {
        ApplicationUser user = (await _userManager.FindByEmailAsync(loginRequest!.Email)) ?? throw new InvalidOperationException("Error");

        var result = await _signInManager.PasswordSignInAsync(user.UserName!, loginRequest.Password, isPersistent: false, lockoutOnFailure: true);
      
        if (!result.Succeeded)
            throw new UnauthorizedAccessException("Invalid credentials");

        RefreshTokenResult refreshTokenResult = await _refreshTokenService.IssueOnLogin(user);

        return (new LoginResponse()
        {
            Id = user.Id,
            UserName = user.UserName!,
            Token = _jwtService.CreateToken(user)
        }, refreshTokenResult);
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

    public async Task<RefreshTokenResponse> RotateRefreshToken(string refreshToken)
    {

        (ApplicationUser user, RefreshTokenResult refreshTokenResult) = await _refreshTokenService.RotateTokenAsync(refreshToken);

        return new RefreshTokenResponse()
        {
            AccessToken = _jwtService.CreateToken(user),
            RefreshToken = refreshTokenResult.RefreshToken,
            ExpirationDate = refreshTokenResult.ExpirationDate
        };
    }

}
