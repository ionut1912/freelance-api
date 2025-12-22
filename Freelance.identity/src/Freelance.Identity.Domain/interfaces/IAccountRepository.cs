using Freelance.Identity.Domain.Entities;
using Shared.Domain.Interfaces;

namespace Freelance.Identity.Domain.interfaces;

public interface IAccountRepository : IGenericRepository<Account>
{
    Task<Account?> GetAccountByUsernameAsync(string username, CancellationToken cancellationToken);
}