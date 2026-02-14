using PrepPal_.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepPal_.Core.DTO.Account;

public class RegisterRequest
{
    [Required] public string UserName { get; set; } = null!;
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string LastName { get; set; } = null!;
    [Required] public string Email { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
    [Required] public string PhoneNumber { get; set; } = null!;


    public ApplicationUser ToUser()
    {
        return new ApplicationUser()
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            UserName = UserName,
            PhoneNumber = PhoneNumber,
        };
    }
}

