using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Interfaces;

namespace Freelance.Identity.Infrastructure.Persistance.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPasswordService _passwordService;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public AccountRepository(ApplicationDbContext dbContext, IPasswordService passwordService,
        IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        ArgumentNullException.ThrowIfNull(passwordService);
        ArgumentNullException.ThrowIfNull(unitOfWork);
        _dbContext = dbContext;
        _passwordService = passwordService;
        _unitOfWork = unitOfWork;
    }

    public async Task RegisterAsync(Account account, CancellationToken cancellationToken)
    {
        account.HashPassword(_passwordService);
        await _dbContext.Accounts.AddAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<Account> LoginAsync(string username, string password)
    {
        var account = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Username == username);
        if (account == null)
            throw new AccountNotFoundException($"Account with username {username} not found");

        return !_passwordService.VerifyPassword(password, account.Password)
            ? throw new PasswordNotMatchException("Passwords do not match")
            : account;
    }

    public async Task<Account> GetAccountAsync(Guid id)
    {
        var account = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == id);
        return account == null ? throw new AccountNotFoundException($"Account with id {id} not found") : account;
    }

    public async Task<bool> ExistsAsync(string username, CancellationToken cancellationToken)
    {
        return await _dbContext.Accounts.AnyAsync(x => x.Username == username, cancellationToken);
    }

    public async Task<Account> GetCurrentAccountAsync(string username)
    {
        var account = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Username == username);
        return account == null ? throw new AccountNotFoundException($"Account with id {username} not found") : account;
    }

    public async Task BlockAccountAsync(Account account, CancellationToken cancellationToken)
    {
        _dbContext.Accounts.Update(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UnblockAccountAsync(Account account, CancellationToken cancellationToken)
    {
        _dbContext.Accounts.Update(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Account account, CancellationToken cancellationToken)
    {
        _dbContext.Accounts.Remove(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}