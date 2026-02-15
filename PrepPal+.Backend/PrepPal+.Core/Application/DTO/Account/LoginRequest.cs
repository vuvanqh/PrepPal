using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepPal_.Core.DTO.Account;

public class LoginRequest
{
    [Required] public string Email { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}
