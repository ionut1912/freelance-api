using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Domain.ValueObjects;
using Freelance.Identity.Infrastructure.Persistance;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.Identity.Application.Mediatr.Accounts.Handlers;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Account>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordService _passwordService;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public CreateAccountCommandHandler(IAccountRepository accountRepository,
        IPasswordService passwordService,IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(accountRepository,nameof(accountRepository));
        ArgumentNullException.ThrowIfNull(passwordService,nameof(passwordService));
        ArgumentNullException.ThrowIfNull(unitOfWork,nameof(unitOfWork));
        _accountRepository = accountRepository;
        _passwordService = passwordService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Account> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var roleForRepo = Role.Client;
        var account = await _accountRepository.GetAccountByUsernameAsync(request.Username, cancellationToken);
        if (account is not null)
            throw new UserAlreadyExistsException($"Account with username {request.Username} already exists");
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
        return accountToCreate;
    }
}