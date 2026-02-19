using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PrepPal_.Core;

public enum CartAccessType
{
    [EnumMember(Value = "owner")]  Owner,
    [EnumMember(Value = "editor")]  Editor,
    [EnumMember(Value = "view")]  Viewer
}
