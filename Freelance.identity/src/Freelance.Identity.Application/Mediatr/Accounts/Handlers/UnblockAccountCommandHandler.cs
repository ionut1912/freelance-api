using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Infrastructure.Persistance;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.Identity.Application.Mediatr.Accounts.Handlers;

public class UnblockAccountCommandHandler : IRequestHandler<UnblockAccountCommand, Unit>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
    private readonly ILogger<UnblockAccountCommandHandler> _logger;

    public UnblockAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<UnblockAccountCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(accountRepository, nameof(accountRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(UnblockAccountCommand request, CancellationToken cancellationToken)
    {
        try 
        {
            var account = await _accountRepository.GetByIdAsync(request.AccountId);
            if (account == null)
            {
                _logger.LogError("We can not unblock account with Id: {AccountId} because does not exists", request.AccountId);
                throw new AccountNotFoundException($"Account with id {request.AccountId} does not exist");
            }

            account.UnblockAccount();
            _accountRepository.Update(account);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        } catch (AccountNotBlockedException ex)
        {
            _logger.LogError(ex, "Account with id: {AccountId} can not be unblocked because it is not blocked", request.AccountId);
            throw;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Account can not be unblocked because we have some problems");
            throw;
        }

        return Unit.Value;
    }
}