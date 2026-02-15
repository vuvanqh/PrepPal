using PrepPal_.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PrepPal_.Core.DTO.Account;

public class LoginResponse
{
    [Required] public Guid Id { get; set; }
    [Required] public string UserName { get; set; } = null!;
    [Required] public string Token { get; set; } = null!;
}
