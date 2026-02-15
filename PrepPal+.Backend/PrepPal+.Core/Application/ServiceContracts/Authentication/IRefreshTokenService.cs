using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.DTO.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.ServiceContracts;

public interface IRefreshTokenService
{
    //double GetMaxSessionLifetimeDays();
    Task<(ApplicationUser, RefreshTokenResult)> RotateTokenAsync(string currentToken);
    Task<RefreshTokenResult> IssueOnLogin(ApplicationUser user);
    string HashToken(string token);
}
