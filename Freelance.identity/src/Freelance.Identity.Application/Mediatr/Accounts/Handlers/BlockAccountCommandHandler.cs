using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Infrastructure.Persistance;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.Identity.Application.Mediatr.Accounts.Handlers;

public class BlockAccountCommandHandler : IRequestHandler<BlockAccountCommand, Unit>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public BlockAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(accountRepository, nameof(accountRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(BlockAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId) ?? throw new AccountNotFoundException($"Account with id {request.AccountId} does not exist");
        account.BlockAccount();
        _accountRepository.Update(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}