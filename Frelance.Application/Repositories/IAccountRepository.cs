using Frelance.Application.Mediatr.Commands.Users;
using Frelance.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Frelance.Application.Repositories;

public interface IAccountRepository
{
    Task RegisterAsync(CreateUserCommand createUserCommand);
    Task<UserDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
}