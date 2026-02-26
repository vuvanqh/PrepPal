using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.AspNetCore.SignalR;
using PrepPal_.Backend.Hubs;
using PrepPal_.Core;
using PrepPal_.Core.Application.DTO;
using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Core.ServiceContracts;
using System.Security.Claims;

namespace PrepPal_.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IUserRepository _userRepo;
    private readonly IRecipeInteractionService _interactionService;
    private readonly IRecipeService _recipeService;
    private readonly IHubContext<NotificationHub, INotificationClient> _hub;

    public CartController(ICartService cartService, IHubContext<NotificationHub, INotificationClient> hub, 
        IUserRepository userRepo, IRecipeInteractionService interactionService, IRecipeService recipeService)
    {
        _cartService = cartService;
        _hub = hub;
        _userRepo = userRepo;
        _interactionService = interactionService;
        _recipeService = recipeService;
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

        await _hub.Clients.Group(request.CartId.ToString()).UpdateCart(request.CartId);
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
        await _hub.Clients.Group(request.CartId.ToString()).UpdateCart(request.CartId);
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

        AccessibleCartsResponse resp = await _cartService.GetAccessibleCartsAsync(Guid.Parse(id));
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

    [HttpPut("clear/{cartId:guid}")]
    public async Task<IActionResult> ClearCart(Guid cartId)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return NotFound();

        try
        {
            await _cartService.ClearCart(Guid.Parse(id), cartId);
        }
        catch (Exception ex)
        {
            return Unauthorized();
        }

        return Ok();
    }

    [HttpPost("invite-to-cart")]
    public async Task<IActionResult> InviteToCart(CartInvitationRequest request)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return Unauthorized("sesison expired");

        ApplicationUser? user = await _userRepo.GetUserByUsernameAsync(request.UserName);
        if (user == null)
            return NotFound("User not found");

        await _cartService.SendInvitation(Guid.Parse(id), request);
        await _hub.Clients.User(user.Id.ToString()).ReceiveCartInvitationNotification(User.Identity!.Name!);
        return Ok("Success");
    }

    [HttpPut("modify-invitation")]
    public async Task<IActionResult> ModifyInvitation(ModifyInvitationStatusRequest request)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return Unauthorized("sesison expired");

        await _cartService.ModifyInvitationStatus(Guid.Parse(id), request);
        
        if(request.Action==ActionType.Accept)
            await _hub.Clients.Group(request.CartId.ToString()).NotifyCartInvitationAccepted(User.Identity!.Name!); //invoke group addition in frontend

        return Ok("Success");
    }

    [HttpPut("modify-access")]
    public async Task<IActionResult> ModifyCartAccess(ModifyCartAccessRequest request)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return Unauthorized("sesison expired");

        ApplicationUser? user = await _userRepo.GetUserByUsernameAsync(request.UserName);
        if (user == null) return BadRequest("user does not exist");

        if (request.Access == CartAccessType.Revoked)
            await _hub.Clients.User(user.Id.ToString()).RemoveFromCart(request.CartId);

        await _cartService.ModifyCartAccess(Guid.Parse(id), request); //invoke group addition in frontend
        return Ok("Success");
    }

    [HttpGet("get-pending-invitations")]
    public async Task<IActionResult> GetPendingInvitations()
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return Unauthorized("sesison expired");

        return Ok(await _cartService.GetPendingInvitations(Guid.Parse(id)));
    }

    [HttpGet("get-recommendations")]
    public async Task<IActionResult> GetRecommendations()
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return Unauthorized("sesison expired");

        RecommendationRequest request = await _interactionService.GetRecommendationRequestData(Guid.Parse(id), InteractionType.Like);
        var client = new HttpClient();

        var resp = await client.PostAsJsonAsync<RecommendationRequest>(new Uri("http://127.0.0.1:8000/recommend"), request);

        if (!resp.IsSuccessStatusCode)
            return StatusCode(502, "Recommendation service failed");

        var recipeIds = await resp.Content.ReadFromJsonAsync<List<Guid>>();

        List<RecipeResponse> recipeResp = new List<RecipeResponse>();

        foreach(var r in recipeIds)
        {
            recipeResp.Add((await _recipeService.GetRecipeById(r)));
        }

        recipeResp = await _recipeService.FillResponseList(recipeResp);
        return Ok(recipeResp);
        
    }

}

