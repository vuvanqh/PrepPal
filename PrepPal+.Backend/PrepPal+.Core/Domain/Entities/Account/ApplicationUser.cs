using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PrepPal_.Core.Domain.Entities;

public class ApplicationUser: IdentityUser<Guid>
{
    [StringLength(100, MinimumLength = 1, ErrorMessage ="First Name must be between 1 and 100 characters.")]
    public required string FirstName { get; set; } = null!;
    [StringLength(100, MinimumLength = 1, ErrorMessage = "First Name must be between 1 and 100 characters.")]
    public string LastName { get; set; } = null!;

    public bool? IsVegan { get; set; } = false;
}
