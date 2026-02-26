using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PrepPal_.Core;

public enum Status
{
    Pending,
    Accepted,
    Rejected
}


public enum ActionType
{
    [EnumMember(Value = "accept")]  Accept,
    [EnumMember(Value = "cancel")]  Cancel,
    [EnumMember(Value = "reject")]  Reject,
    [EnumMember(Value = "remove")]  Remove,
    [EnumMember(Value = "block")]  Block,
    [EnumMember(Value = "edit")] Edit
}