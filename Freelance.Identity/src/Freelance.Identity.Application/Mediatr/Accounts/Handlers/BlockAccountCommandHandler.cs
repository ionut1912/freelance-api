using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.Identity.Application.Mediatr.Accounts.Handlers;

public class BlockAccountCommandHandler : IRequestHandler<BlockAccountCommand, Unit>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BlockAccountCommandHandler> _logger;

    public BlockAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork,ILogger<BlockAccountCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(accountRepository, nameof(accountRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(BlockAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var account = await _accountRepository.GetByIdAsync(request.AccountId) ?? throw new AccountNotFoundException($"Account with id {request.AccountId} does not exist");
            account.BlockAccount();
            _accountRepository.Update(account);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (AccountNotFoundException ex)
        {
            _logger.LogError(ex, "Account with Id: {AccountId} can not be blocked because was not found", request.AccountId);
            throw;
        }
        catch (AccountAlreadyBlockedException ex)
        {
            _logger.LogError(ex, "Account with Id : {AccountId} can not be blocked because it is already blocked", request.AccountId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Account with Id: {AccountId} can not be blocked because an error occured", request.AccountId);
            throw;
        }
        _logger.LogInformation("Account with Id : {AccountId} was blocked",request.AccountId);

        return Unit.Value;
    }
}