using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepPal_.Core;

public class CartIdListResponse
{
    [Required] public List<Guid> CartIdList = new List<Guid>();
}
