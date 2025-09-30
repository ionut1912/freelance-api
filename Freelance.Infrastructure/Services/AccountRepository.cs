using Freelance.Application.Mediatr.Commands.Users;
using Freelance.Application.Repositories;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Errors;
using Freelance.Contracts.Exceptions;
using Freelance.Infrastructure.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Freelance.Infrastructure.Services;

public class AccountRepository : IAccountRepository
{
    private readonly TokenService _tokenService;
    private readonly UserManager<Users> _userManager;
    private readonly IUserAccessor _userAccessor;

    public AccountRepository(UserManager<Users> userManager, TokenService tokenService,IUserAccessor userAccessor)
    {
        ArgumentNullException.ThrowIfNull(userManager, nameof(userManager));
        ArgumentNullException.ThrowIfNull(tokenService, nameof(tokenService));
        ArgumentNullException.ThrowIfNull(userAccessor,nameof(userAccessor));
        _userManager = userManager;
        _tokenService = tokenService;
        _userAccessor = userAccessor;
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
        if (user is null)
        {
            modelState.AddModelError("Username", $"User with username {loginDto.Username} is not found");
            GenerateException(modelState);
        }

        if (!await _userManager.CheckPasswordAsync(user!, loginDto.Password))
            modelState.AddModelError("Password", "password is invalid");

        if (await _userManager.IsLockedOutAsync(user!))
            modelState.AddModelError("Account", "Your account is locked for one hour");

        GenerateException(modelState);
        return new UserDto(user!.PhoneNumber!, await _tokenService.GenerateToken(user), user.UserName!, user.Email!,
            user.CreatedAt);
    }

    public async Task LockAccountAsync(BlockAccountCommand command)
    {
        var modelState = new ModelStateDictionary();
        var user = await _userManager.FindByIdAsync(command.UserId) ??
                   throw new NotFoundException($"{nameof(Users)} with id {command.UserId} not found");
        var lockoutEnd = DateTimeOffset.UtcNow.AddHours(1);
        var result = await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);
        if (!result.Succeeded)
        {
            modelState.AddModelError("User", "Failed to lock account");
            GenerateException(modelState);
        }
    }

    public async Task DeleteCurrentAccountAsync(DeleteCurrentAccountCommand command, CancellationToken cancellationToken)
    {
        var modelState = new ModelStateDictionary();

        var user = await _userManager.Users
                       .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken)
                   ?? throw new NotFoundException($"Users with username {_userAccessor.GetUsername()} not found");

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
            await _userManager.RemoveFromRoleAsync(user, role);

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            modelState.AddModelError("User", string.Join("; ", result.Errors.Select(e => e.Description)));
            GenerateException(modelState);
        }
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