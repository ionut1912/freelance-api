using Freelance.Application.Mediatr.Commands.Users;
using Freelance.Contracts.Dtos;

namespace Freelance.Application.Repositories;

public interface IAccountRepository
{
    Task RegisterAsync(CreateUserCommand createUserCommand);
    Task<UserDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
    Task LockAccountAsync(BlockAccountCommand command);
    Task DeleteCurrentAccountAsync(DeleteCurrentAccountCommand command,CancellationToken cancellationToken);
}