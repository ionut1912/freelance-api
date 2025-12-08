using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Domain.ValueObjects;
using Shared.Application.Mediator;

namespace Freelance.Identity.Application.Mediatr.Accounts.Handlers;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Account>
{
    private readonly IAccountRepository _accountRepository;

    public CreateAccountCommandHandler(IAccountRepository accountRepository)
    {
        ArgumentNullException.ThrowIfNull(accountRepository);
        _accountRepository = accountRepository;
    }

    public async Task<Account> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var roleForRepo = Role.Client;
        var existsByUsername = await _accountRepository.ExistsAsync(request.Username, cancellationToken);
        if (existsByUsername)
            throw new UserAlreadyExistsException($"Account with username {request.Username} already exists");
        roleForRepo = request.Role switch
        {
            "Client" => Role.Client,
            "Freelancer" => Role.Freelancer,
            _ => roleForRepo
        };

        var account = Account.Create(request.Email, request.Password, request.PhoneNumber, request.Username,
            roleForRepo);
        await _accountRepository.RegisterAsync(account, cancellationToken);
        return account;
    }
}