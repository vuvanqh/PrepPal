using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public enum Status
{
    Pending,
    Accepted,
}


public enum ConnectionAction
{
    Accept,
    Cancel,
    Reject,
    Remove,
    Block
}