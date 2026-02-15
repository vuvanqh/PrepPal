using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.Application.Errors;

public class ExternalServiceException: Exception
{
    public ExternalServiceException(string message): base(message) { }
}
