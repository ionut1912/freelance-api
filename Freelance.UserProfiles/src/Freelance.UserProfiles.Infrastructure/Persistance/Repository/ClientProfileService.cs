using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Infra.Services;

namespace Freelance.UserProfiles.Infrastructure.Persistance.Repository;

public class ClientProfileService(DbSet<ClientProfile> dbSet) : GenericRepository<ClientProfile>(dbSet), IClientProfileRepository
{
    public async Task<ClientProfile> GetLoggedInClientProfileAsync(Guid accountId,
        CancellationToken cancellationToken)
    {
        var clientProfile = await dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AccountId == accountId, cancellationToken);
        return clientProfile == null
            ? throw new ProfileNotFoundException($"Profile with AccountId {accountId} not found")
            : clientProfile;
    }
}