using Frelance.Application.Mediatr.Commands.Users;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Errors;
using Frelance.Contracts.Exceptions;
using Frelance.Infrastructure.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services;

public class AccountRepository : IAccountRepository
{
    private readonly TokenService _tokenService;
    private readonly UserManager<Users> _userManager;

    public AccountRepository(UserManager<Users> userManager, TokenService tokenService)
    {
        ArgumentNullException.ThrowIfNull(userManager, nameof(userManager));
        ArgumentNullException.ThrowIfNull(tokenService, nameof(tokenService));
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task RegisterAsync(CreateUserCommand createUserCommand)
    {
        var modelState = new ModelStateDictionary();

        var existingUserByName = await _userManager.FindByNameAsync(createUserCommand.RegisterDto.Username);
        if (existingUserByName != null) modelState.AddModelError("Username", "username is already taken");

        var existingUserByEmail = await _userManager.FindByEmailAsync(createUserCommand.RegisterDto.Email);
        if (existingUserByEmail != null) modelState.AddModelError("Email", "email is already taken");
        GenerateException(modelState);

        var user = createUserCommand.Adapt<Users>();

        var result = await _userManager.CreateAsync(user, createUserCommand.RegisterDto.Password);
        if (!result.Succeeded) AddErrorToModelState(result, modelState);
        GenerateException(modelState);

        await _userManager.AddToRoleAsync(user, createUserCommand.RegisterDto.Role);
    }


    public async Task<UserDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var modelState = new ModelStateDictionary();
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username,
            cancellationToken);
        if (user is null) modelState.AddModelError("Username", "The given user data is not found");

        if (user!.UserName != loginDto.Username) modelState.AddModelError("Username", "username is invalid");

        if (!await _userManager.CheckPasswordAsync(user!, loginDto.Password))
            modelState.AddModelError("Password", "password is invalid");

        if (user.Email != loginDto.Email) modelState.AddModelError("Email", "email is invalid");

        if (await _userManager.IsLockedOutAsync(user))
            modelState.AddModelError("Account", "Your account is locked for one hour");

        GenerateException(modelState);
        return new UserDto(user.PhoneNumber!, await _tokenService.GenerateToken(user), user.UserName!, user.Email!,
            user.CreatedAt);
    }

    public async Task LockAccountAsync(BlockAccountCommand command, CancellationToken cancellationToken)
    {
        var modelState = new ModelStateDictionary();
        var user = await _userManager.FindByIdAsync(command.UserId);
        if (user is null) throw new NotFoundException($"{nameof(Users)} with id {command.UserId} not found");
        var lockoutEnd = DateTimeOffset.UtcNow.AddHours(1);
        var result = await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);
        if (!result.Succeeded)
        {
            modelState.AddModelError("User", "Failed to lock account");
            GenerateException(modelState);
        }
    }

    public async Task DeleteAccountAsync(DeleteAccountCommand command, CancellationToken cancellationToken)
    {
        var modelState = new ModelStateDictionary();
        var user = await _userManager.FindByIdAsync(command.UserId);
        if (user is null) throw new NotFoundException($"{nameof(Users)} with id {command.UserId} not found");
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            modelState.AddModelError("User", "Failed to delete account");
            GenerateException(modelState);
        }

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles) await _userManager.RemoveFromRoleAsync(user, role);
    }

    private static void AddErrorToModelState(IdentityResult result, ModelStateDictionary modelState)
    {
        foreach (var error in result.Errors) modelState.AddModelError(error.Code, error.Description);
    }

    private static void GenerateException(ModelStateDictionary modelState)
    {
        if (modelState.IsValid) return;
        var validationErrors = modelState
            .Where(kvp => kvp.Value!.Errors.Count > 0)
            .SelectMany(kvp => kvp.Value!.Errors.Select(error => new ValidationError(kvp.Key, error.ErrorMessage)))
            .ToList();
        throw new CustomValidationException(validationErrors);
    }
}