using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Infra.Services;

namespace Freelance.Identity.Infrastructure.Persistance.Repositories;

public sealed class AccountRepository(DbSet<Account> dbSet) : GenericRepository<Account>(dbSet), IAccountRepository
{
    private readonly DbSet<Account> _accounts = dbSet;

    public Task<Account?> GetAccountByUsernameAsync(string username, CancellationToken cancellationToken) =>
        _accounts.AsNoTracking().FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
}
