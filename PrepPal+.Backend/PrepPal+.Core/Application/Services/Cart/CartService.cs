using PrepPal_.Core.Application.DTO;
using PrepPal_.Core.Application.Services;
using PrepPal_.Core.Domain;
using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepPal_.Core;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepo;
    private readonly IRecipeRepository _recipeRepo;
    private readonly IRecipeService _recipeService;
    private readonly ICartInvitationRepository _cartInvitationRepository;
    private readonly IUserRepository _userRepository;
    private readonly CartInvitationPolicy _cartInvitationPolicy;

    public CartService(ICartRepository cartRepo, IRecipeRepository recipeRepo, IRecipeService recipeService,
        CartInvitationPolicy policy, ICartInvitationRepository cartInvitationRepository, IUserRepository userRepository)
    {
        _cartRepo = cartRepo;
        _recipeRepo = recipeRepo;
        _recipeService = recipeService;
        _cartInvitationPolicy = policy;
        _cartInvitationRepository = cartInvitationRepository;
        _userRepository = userRepository;
    }

    public async Task AddToCart(Guid userId, Guid cartId, int externalId)
    {
        bool? canEdit = await _cartRepo.HasPermission(userId, cartId, CartAccessType.Editor);
        if (!canEdit.HasValue || !canEdit.Value)
            throw new UnauthorizedAccessException("No permission");

        Guid recipeId = await _recipeService.EnsureRecipeExistsAsync(externalId);

        await _cartRepo.AddToCartAsync(cartId, userId, recipeId);
    }


    public async Task RemoveFromCart(Guid userId, Guid cartId, int externalId)
    {
        bool? canEdit = await _cartRepo.HasPermission(userId, cartId, CartAccessType.Editor);
        if (!canEdit.HasValue || !canEdit.Value)
            throw new UnauthorizedAccessException("No permission");

        Recipe? r = await _recipeRepo.GetRecipeAsync(externalId);
        if (r == null) {
            Console.WriteLine("Null recipe");
            return;
        };

        await _cartRepo.RemoveFromCartAsync(userId, cartId, r.Id);
    }


    public async Task CreateCart(Guid userId)
    {
        if ((await _cartRepo.GetOwnedCartsAsync(userId))?.Count() > 10)
            throw new InvalidOperationException("You own too many carts. Delete some before creating a new one");
        await _cartRepo.CreateCart(userId);
    }

    public async Task DeleteCart(Guid userId, Guid cartId)
    {
        bool? owns = await _cartRepo.HasPermission(userId, cartId, CartAccessType.Owner);
        if (!owns.HasValue || !owns.Value)
            throw new UnauthorizedAccessException("No permission");

        await _cartRepo.DeleteCart(userId, cartId);
    }

    public async Task<AccessibleCartsResponse> GetAccessibleCartsAsync(Guid userId) //excluding the owned one
    {
        List<Cart> carts = await _cartRepo.GetAccessibleCartsAsync(userId);

        return new AccessibleCartsResponse()
        {
            Carts = carts.Select(c => new AccessibleCart()
            {
                CartId = c.Id,
                OwnerUserName = c.Owner.UserName!
            }).ToList()
        };
    }

    public async Task ClearCart(Guid userId, Guid cartId)
    {
        bool? owns = await _cartRepo.HasPermission(userId, cartId, CartAccessType.Editor);
        if (!owns.HasValue || !owns.Value )
            throw new UnauthorizedAccessException("No permission");

        await _cartRepo.ClearCart(cartId);
    }
    public async Task<CartIdListResponse> GetOwnedCartsAsync(Guid userId)
    {
       List<Cart>? carts = await _cartRepo.GetOwnedCartsAsync(userId);
       return new CartIdListResponse() { CartIdList = carts?.Select(c => c.Id)?.ToList() ?? new List<Guid>() };
    }

    public async Task<CartResponse?> GetCartAsync(Guid userId, Guid cartId)
    {

        bool? owns = await _cartRepo.HasPermission(userId, cartId, CartAccessType.Viewer);
        if (!owns.HasValue || !owns.Value)
            throw new UnauthorizedAccessException("No permission");

        Cart? c = await _cartRepo.GetCartByIdAsync(userId, cartId);
        return c?.ToCartRecipeResponse();
    }

    public async Task<CartResponse?> GetCartContent(Guid userId, Guid cartId)
    {
        CartResponse? c = await _cartRepo.GetCartDetailsAsync(userId, cartId);
       
        return c;
    }
    public async Task SendInvitation(Guid userId, CartInvitationRequest request)
    {
        await _cartRepo.HasPermission(userId, request.CartId, CartAccessType.Editor);
        ApplicationUser? user = await _userRepository.GetUserByUsernameAsync(request.UserName);
        if (user == null)
            throw new ArgumentException("User does not exist");

        CartInvitation invitation = new CartInvitation() {
            AccessType = request.Access,
            Id = Guid.NewGuid(),
            CartId = request.CartId,
            ReceiverId = user.Id,
            SenderId = userId,
            Timestamp = DateTime.UtcNow,
            Status = Status.Pending
        };

        await _cartInvitationRepository.AddInvitation(invitation);
    }
    public async Task ModifyInvitationStatus(Guid userId, ModifyInvitationStatusRequest request)
    {
        CartInvitation? invitation = await _cartInvitationRepository.GetInvitationById(request.InvitationId);
        if (invitation == null) 
            throw new InvalidOperationException("no request");
        await _cartInvitationPolicy.EnsureCanModifyStatus(userId, invitation);

        Status s;
        if (request.Action == ActionType.Accept)
        {
            s = Status.Accepted;
            await _cartRepo.GiveAccessAsync(invitation.ReceiverId, invitation.CartId, invitation.AccessType);
        }
        else
            s = Status.Rejected;

        await _cartInvitationRepository.UpdateInvitation(request.InvitationId, s);
    }

    public async Task ModifyCartAccess(Guid userId, ModifyCartAccessRequest access)
    {
        Cart? c = await _cartRepo.GetCartByIdAsync(userId, access.CartId);
        if(c==null)
            throw new InvalidOperationException("no invalid cart");

        bool? canEdit = await _cartRepo.HasPermission(userId, c.Id, CartAccessType.Owner);

        if (!canEdit.HasValue || !canEdit.Value)
            throw new UnauthorizedAccessException("no permissions");

        await _cartRepo.UpdateAccess(userId, access.CartId, access.Access);
        //await _cartRe
    }

    public async Task<List<CartInvitationResponse>> GetPendingInvitations(Guid userId)
    {
        List<CartInvitation> invitations = await _cartInvitationRepository.GetUserInvitation(userId, Status.Pending);

        return invitations.Select(i => new CartInvitationResponse()
        {
            InvitationId = i.Id,
            CartId = i.CartId,
            OwnerUserName = i.Cart.Owner.UserName!
        }).ToList();
    }

    
}
