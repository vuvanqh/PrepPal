using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepPal_.Core;


public class AccessibleCartsResponse
{
    [Required] public List<AccessibleCart> Carts { get; set; } = new List<AccessibleCart>();
}


public class AccessibleCart
{
    [Required] public Guid CartId { get; set; }
    [Required] public string OwnerUserName { get; set; } = null!;
}
