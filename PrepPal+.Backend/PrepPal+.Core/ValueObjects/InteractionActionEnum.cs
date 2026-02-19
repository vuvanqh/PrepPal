using System.Runtime.Serialization;

namespace PrepPal_.Core;
public enum InteractionAction
{
    [EnumMember(Value = "add")] Add,
    [EnumMember(Value = "remove")] Remove,
    [EnumMember(Value = "update")] Update
}

public enum InteractionType
{
    [EnumMember(Value = "view")] View,
    [EnumMember(Value = "like")] Like,
    [EnumMember(Value = "add-to-cart")] AddToCart,
    [EnumMember(Value = "unlike")] Unlike,
    [EnumMember(Value = "remove-from-cart")] RemoveFromCart
}