using PrepPal_.Core.Application.DTO;
using PrepPal_.Core.Application.DTO.Recipes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepPal_.Core;

public class CartResponse
{
    [Required] public Guid CartId { get; set; }
    [Required] public string OwnerUserName { get; set; } = null!;
    [Required] public List<CartAccessDTO> Members { get; set; } = new List<CartAccessDTO>();
    [Required] public List<CartRecipeResponse> CartRecipes { get; set; } = new List<CartRecipeResponse>();

}

public class CartAccessDTO
{
    [Required] public string UserName { get; set; } = null!;
    [Required] public CartAccessType AccessType {  get; set; }
}


public static class CartAccessExtention{
    public static CartAccessDTO ToCartAccessDTO(this CartAccess access)
    {
        return new CartAccessDTO()
        {
            AccessType = access.AccessType,
            UserName = access.User.UserName!
        };
    }
}

public static class CartExtention
{
    public static CartResponse ToCartRecipeResponse(this Cart cart)
    {
        List<CartAccessDTO>? accesses = cart.Accesses.Where(c => c.CartId == cart.Id).ToList().Select(c=>c.ToCartAccessDTO()).ToList();
        return new CartResponse()
        {
            CartId = cart.Id,
            OwnerUserName = cart.Owner.UserName!,
            Members = accesses,
            CartRecipes = cart.Recipes.Select(r=> new CartRecipeResponse()
            {
                Recipe = r.Recipe.ToRecipeResponse(),
                Quantity = r.Quantity
            }
            ).ToList(),
        };
    }
}