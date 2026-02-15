using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.DTO.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.ServiceContracts;

/// <summary>
/// To-Do: -change password, create group, add members, invite via link???, delete gc, delete acc
/// </summary>
public interface IAuthenticationService
{
    Task<ApplicationUser> Register(RegisterRequest? registerRequest);
    Task<(LoginResponse, RefreshTokenResult)> Login(LoginRequest? loginRequest);

    Task<RefreshTokenResponse> RotateRefreshToken(string refreshToken);

}
