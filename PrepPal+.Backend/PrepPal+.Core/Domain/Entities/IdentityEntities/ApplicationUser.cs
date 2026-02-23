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
    public string? TokenHash { get; set; }
    public DateTime TokenExpirationDate {  get; set; }
    public DateTime TokenIssuedAt { get; set; }
    public bool? IsVegan { get; set; } = false;


    public ICollection<Cart> Carts {get;set;} = new List<Cart>();
    public ICollection<CartAccess> Accesses { get; set; } = new List<CartAccess>();

}
