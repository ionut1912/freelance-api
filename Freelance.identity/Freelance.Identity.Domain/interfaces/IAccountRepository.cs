using Freelance.Identity.Domain.Entities;

namespace Freelance.Identity.Domain.interfaces;

public interface IAccountRepository
{
    Task RegisterAsync(Account account, CancellationToken cancellationToken);
    Task<Account> LoginAsync(string username, string password);
    Task<Account> GetAccountAsync(Guid id);
    Task<bool> ExistsAsync(string username, CancellationToken cancellationToken);
    Task<Account> GetCurrentAccountAsync(string username);
    Task BlockAccountAsync(Account account, CancellationToken cancellationToken);
    Task UnblockAccountAsync(Account account,CancellationToken cancellationToken);
    Task DeleteAsync(Account account, CancellationToken cancellationToken);
}