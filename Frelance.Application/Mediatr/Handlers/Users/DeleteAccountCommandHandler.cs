using Frelance.Application.Mediatr.Commands.Users;
using Frelance.Application.Repositories;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Users;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand>
{
    private readonly IAccountRepository _accountRepository;

    public DeleteAccountCommandHandler(IAccountRepository accountRepository)
    {
        ArgumentNullException.ThrowIfNull(accountRepository, nameof(accountRepository));
        _accountRepository = accountRepository;
    }

    public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        await _accountRepository.DeleteAccountAsync(request);
    }
}