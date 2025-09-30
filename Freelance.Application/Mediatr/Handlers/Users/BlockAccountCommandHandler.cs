using Freelance.Application.Mediatr.Commands.Users;
using Freelance.Application.Repositories;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.Users;

public class BlockAccountCommandHandler : IRequestHandler<BlockAccountCommand>
{
    private readonly IAccountRepository _accountRepository;

    public BlockAccountCommandHandler(IAccountRepository accountRepository)
    {
        ArgumentNullException.ThrowIfNull(accountRepository, nameof(accountRepository));
        _accountRepository = accountRepository;
    }

    public async Task Handle(BlockAccountCommand request, CancellationToken cancellationToken)
    {
        await _accountRepository.LockAccountAsync(request);
    }
}