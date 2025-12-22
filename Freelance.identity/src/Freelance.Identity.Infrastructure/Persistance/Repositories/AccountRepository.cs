using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Infra.Services;

namespace Freelance.Identity.Infrastructure.Persistance.Repositories;

public class AccountRepository(DbSet<Account> dbSet) : GenericRepository<Account>(dbSet), IAccountRepository
{
    public async Task<Account?> GetAccountByUsernameAsync(string username, CancellationToken cancellationToken)
    {

        return await dbSet.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
    }
}
