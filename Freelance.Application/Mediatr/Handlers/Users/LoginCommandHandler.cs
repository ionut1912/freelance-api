using Freelance.Application.Mediatr.Commands.Users;
using Freelance.Application.Repositories;
using Freelance.Contracts.Dtos;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.Users;

public class LoginCommandHandler : IRequestHandler<LoginCommand, UserDto>
{
    private readonly IAccountRepository _accountRepository;

    public LoginCommandHandler(IAccountRepository accountRepository)
    {
        ArgumentNullException.ThrowIfNull(accountRepository, nameof(accountRepository));
        _accountRepository = accountRepository;
    }

    public async Task<UserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await _accountRepository.LoginAsync(request.LoginDto, cancellationToken);
    }
}