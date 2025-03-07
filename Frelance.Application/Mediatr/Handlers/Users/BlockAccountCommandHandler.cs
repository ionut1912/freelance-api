using Frelance.Application.Mediatr.Commands.Users;
using Frelance.Application.Repositories;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Users;

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
        await _accountRepository.LockAccountAsync(request, cancellationToken);
    }
}