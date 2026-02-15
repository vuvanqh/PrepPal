using PrepPal_.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.ServiceContracts;

public interface IJwtService
{
    string CreateToken(ApplicationUser user);
}
