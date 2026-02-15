using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PrepPal_.Core.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(ApplicationUser user)
    {
        DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:expiration_minutes"]));

        Claim[] claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), //subject = user identity
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //unique token id
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()), //issued at
            new Claim(ClaimTypes.NameIdentifier, user.Email!) //unique value of a user
        };

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)); //security key
        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); //hashing
        JwtSecurityToken tokenGenerator = new JwtSecurityToken(
            _configuration["Jwt:Issuer"], //from who
            _configuration["Jwt:Audience"], //to who
            claims,
            expires: expiration,
            signingCredentials: signingCredentials);

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

        return handler.WriteToken(tokenGenerator);
    }
}
