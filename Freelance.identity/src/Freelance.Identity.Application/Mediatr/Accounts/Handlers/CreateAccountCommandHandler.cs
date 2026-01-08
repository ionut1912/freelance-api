using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Domain.ValueObjects;
using Freelance.Identity.Infrastructure.Persistance;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.Identity.Application.Mediatr.Accounts.Handlers;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Account>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordService _passwordService;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
    private readonly ILogger<CreateAccountCommandHandler> _logger;

    public CreateAccountCommandHandler(IAccountRepository accountRepository,
        IPasswordService passwordService, IUnitOfWork<ApplicationDbContext> unitOfWork,ILogger<CreateAccountCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(accountRepository, nameof(accountRepository));
        ArgumentNullException.ThrowIfNull(passwordService, nameof(passwordService));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _accountRepository = accountRepository;
        _passwordService = passwordService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Account> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var roleForRepo = Role.Client;
        var account = await _accountRepository.GetAccountByUsernameAsync(request.Username, cancellationToken);
        if (account is not null)
        {
            _logger.LogError("Accont with username :{Username} can not be created because already exists", request.Username);
            throw new UserAlreadyExistsException($"Account with username {request.Username} already exists");
        }

        roleForRepo = request.Role switch
        {
            "Client" => Role.Client,
            "Freelancer" => Role.Freelancer,
            _ => roleForRepo
        };

        var accountToCreate = Account.Create(request.Email, request.Password, request.PhoneNumber, request.Username,
            roleForRepo);
        accountToCreate.HashPassword(_passwordService);
        await _accountRepository.AddAsync(accountToCreate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Account was created");
        return accountToCreate;
    }
}