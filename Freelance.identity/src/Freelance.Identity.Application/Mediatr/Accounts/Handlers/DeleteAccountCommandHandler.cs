using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Infrastructure.Persistance;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.Identity.Application.Mediatr.Accounts.Handlers;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, Unit>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public DeleteAccountCommandHandler(IAccountRepository accountRepository,IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(accountRepository,nameof(accountRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork,nameof(unitOfWork));
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.Id);
        if (account == null)
        {
            throw new AccountNotFoundException($"Account with Id {request.Id} was not found.");
        }
        _accountRepository.Delete(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}