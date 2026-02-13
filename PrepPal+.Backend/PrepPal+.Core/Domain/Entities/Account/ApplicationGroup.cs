using PrepPal_.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepPal_.Core.Domain.Entities;

public class ApplicationGroup
{
    [Key]
    public required Guid GroupId { get; set; } 
    public required string Name { get; set; }
    public required Guid OwnerId { get; set; }
}
