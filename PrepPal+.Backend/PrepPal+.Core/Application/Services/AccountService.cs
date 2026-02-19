using PrepPal_.Core.Application.DTO;
using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Core.DTO.Account;
using PrepPal_.Core.Errors;
using PrepPal_.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.Services;

public class AccountService : IAccountService
{
    private readonly IUserRepository _userRepository;

    public AccountService(IUserRepository userRepository)
    {
        _userRepository = userRepository; 
    }

    public async Task<PersonalDetailsResponse> GetPersonalDetails(Guid userId)
    {
        ApplicationUser? user = await _userRepository.GetUserById(userId) ?? throw new ArgumentNullException("User does not exist");

        return new PersonalDetailsResponse()
        {
            UserName = user.UserName!,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber!,
            FirstName = user.FirstName,
            LastName = user.LastName,
        };
    }
}
