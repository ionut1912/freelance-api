using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Infra.Services;

namespace Freelance.UserProfiles.Infrastructure.Persistance.Repository;

public class FreelancerProfileService(DbSet<FreelancerProfile> dbSet) : GenericRepository<FreelancerProfile>(dbSet), IFreelancerProfileRepository
{

    public async Task<FreelancerProfile> GetLoggedInFreelancerProfileAsync(Guid accountId,
        CancellationToken cancellationToken)
    {
        var freelancerProfile = await dbSet
            .AsNoTracking()
            .Include(f => f.ForeignLanguages)
            .Include(f => f.Skills)
            .FirstOrDefaultAsync(x => x.AccountId == accountId, cancellationToken);
        return freelancerProfile == null
            ? throw new ProfileNotFoundException($"Profile with accountId {accountId} not found")
            : freelancerProfile;
    }
}