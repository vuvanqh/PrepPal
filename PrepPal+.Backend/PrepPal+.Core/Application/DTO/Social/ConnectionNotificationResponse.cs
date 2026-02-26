using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepPal_.Core;

public class ConnectionNotificationResponse
{
    [Required] public string UserName { get; set; } = null!;
    [Required] public ActionType Action { get; set; }
}
