using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrepPal_.Core;
using System.Security.Claims;

namespace PrepPal_.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    public class CartRecipeRequest
    {
        public Guid CartId { get; set; }
        public int ExternalId { get; set; }
    }

    [HttpPost("add-recipe")]
    public async Task<IActionResult> AddToCart(CartRecipeRequest request)
    {
        Console.WriteLine(request.CartId);
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return NotFound();

        await _cartService.AddToCart(Guid.Parse(id), request.CartId, request.ExternalId);
        return Ok();
    }

    [HttpPost("remove-recipe")]
    public async Task<IActionResult> RemoveFromCart(CartRecipeRequest request)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return NotFound();
        Console.WriteLine("hey");
        await _cartService.RemoveFromCart(Guid.Parse(id), request.CartId, request.ExternalId);
        return Ok();
    }

    [HttpGet("get-content/{cartId:guid}")]
    public async Task<IActionResult> GetCartContents(Guid cartId)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return NotFound();

        CartResponse? c = await _cartService.GetCartContent(Guid.Parse(id), cartId);
        return Ok(c);
    }

    [HttpGet("get-owned")]
    public async Task<IActionResult> GetOwned()
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine("JWT UserId: " + id);
        if (id == null)
            return NotFound();
       
        CartIdListResponse resp = await _cartService.GetOwnedCartsAsync(Guid.Parse(id));
        Console.WriteLine(resp.CartIdList[0]);
        return Ok(resp);
    }

    [HttpGet("get-accessible")]
    public async Task<IActionResult> GetAccessible()
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return NotFound();

        CartIdListResponse resp = await _cartService.GetAccessibleCartsAsync(Guid.Parse(id));
        return Ok(resp);
    }

    [HttpGet("get-cart/{cartId:guid}")]
    public async Task<IActionResult> GetCart(Guid cartId)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return NotFound();

        CartResponse? resp = await _cartService.GetCartAsync(Guid.Parse(id), cartId);
        return Ok(resp);
    }

    [HttpDelete("delete-cart/{cartId:guid}")]
    public async Task<IActionResult> DeleteCart(Guid cartId)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return NotFound();

        await _cartService.DeleteCart(Guid.Parse(id), cartId);
        return Ok();
    }


    [HttpPost("create-cart")]
    public async Task<IActionResult> Create()
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return NotFound();

        await _cartService.CreateCart(Guid.Parse(id));
        return Ok();
    }

}
