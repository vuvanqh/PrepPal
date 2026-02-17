using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepPal_.Core.DTO.Account;

public class PersonalDetailsResponse
{
    [Required] public string UserName { get; set; } = null!;
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string LastName { get; set; } = null!;
    [Required] public string Email { get; set; } = null!;
    [Required] public string PhoneNumber { get; set; } = null!;
}
