using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Infra.Services;

namespace Freelance.UserProfiles.Infrastructure.Persistance.Repository;

public sealed class ClientProfileService(DbSet<ClientProfile> dbSet)
    : GenericRepository<ClientProfile>(dbSet), IClientProfileRepository
{
    private readonly DbSet<ClientProfile> _profiles = dbSet;

    public Task<ClientProfile?> GetLoggedInClientProfileAsync(Guid accountId, CancellationToken cancellationToken) =>
        _profiles.AsNoTracking().FirstOrDefaultAsync(x => x.AccountId == accountId, cancellationToken);
}
