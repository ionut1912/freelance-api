using Frelance.Application.Mediatr.Commands.Users;
using Frelance.Application.Repositories;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Frelance.Infrastructure.Services;

public class AccountRepository:IAccountRepository
{
    private readonly FrelanceDbContext _dbContext;
    private readonly UserManager<Users> _userManager;
    private readonly TokenService _tokenService;

    public AccountRepository(FrelanceDbContext dbContext, UserManager<Users> userManager, TokenService tokenService)
    {
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
        ArgumentNullException.ThrowIfNull(userManager, nameof(userManager));
        ArgumentNullException.ThrowIfNull(tokenService, nameof(tokenService));
        _dbContext = dbContext;
        _userManager = userManager;
        _tokenService = tokenService;
    }
    public async Task RegisterAsync(CreateUserCommand createUserCommand)
    {
        var user = new Users
        {
            Email = createUserCommand.RegisterDto.Email,
            UserName = createUserCommand.RegisterDto.Username,
            PhoneNumber = createUserCommand.RegisterDto.PhoneNumber,
        };
        var result=await _userManager.CreateAsync(user, createUserCommand.RegisterDto.Password);
        if (!result.Succeeded)
        {
            AddErrorToModelState(result, createUserCommand.ModelStateDictionary);
        }
        await _userManager.AddToRoleAsync(user,createUserCommand.RegisterDto.Role);

    }

    private static void AddErrorToModelState(IdentityResult result, ModelStateDictionary modelState)
    {
        foreach (var error in result.Errors)
        {
            modelState.AddModelError(error.Code, error.Description);
            
        }
    }
}