using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Core.DTO.Account;
using PrepPal_.Core.Errors;
using PrepPal_.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PrepPal_.Core.Services;

public class RefreshTokenService: IRefreshTokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;
    public RefreshTokenService(IConfiguration configuration, UserManager<ApplicationUser> userManager, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public async Task<(ApplicationUser, RefreshTokenResult)> RotateTokenAsync(string currentToken)
    {
        ApplicationUser user = await _userRepository.GetUserByRefreshToken(HashToken(currentToken));

        if (user == null)
            throw new RefreshTokenException("Invalid Token");

        if (user.TokenExpirationDate < DateTime.UtcNow)
            throw new RefreshTokenException("Refresh token expired");

        if (user.TokenIssuedAt.AddDays(GetMaxSessionLifetimeDays()) < DateTime.UtcNow)
            throw new RefreshTokenException("Session Expired");

        return (user ,await IssueAndPersistChanges(user));
    }
    public async Task<RefreshTokenResult> IssueOnLogin(ApplicationUser user)
    {
        user.TokenIssuedAt = DateTime.UtcNow;

        return await IssueAndPersistChanges(user);
    }


    //helpers
    private RefreshTokenResult GenerateRefreshToken()
    {
        byte[] bytes = new byte[64];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(bytes);

        DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["RefreshToken:expiration_minutes"]));
        return new RefreshTokenResult() {
            RefreshToken= Convert.ToBase64String(bytes), 
            ExpirationDate = expiration };
    }

    private async Task<RefreshTokenResult> IssueAndPersistChanges(ApplicationUser user)
    {
        RefreshTokenResult result = GenerateRefreshToken();
        user.TokenHash = HashToken(result.RefreshToken);
        user.TokenExpirationDate = result.ExpirationDate;
        await _userManager.UpdateAsync(user);

        return result;
    }

    public double GetMaxSessionLifetimeDays() => Convert.ToInt32(_configuration["RefreshToken:max_session_lifetime_days"]);

    public string HashToken(string token)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(token);
        return Convert.ToBase64String(sha256.ComputeHash(bytes));
    }
}
