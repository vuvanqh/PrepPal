using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.Errors;

public class RefreshTokenException: Exception
{
    public RefreshTokenException(string message): base(message) { } 
}
