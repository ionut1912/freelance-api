using System.Linq.Expressions;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Infra.Services;

namespace Freelance.UserProfiles.Infrastructure.Persistance.Repository;

public sealed class FreelancerProfileService(DbSet<FreelancerProfile> dbSet)
    : GenericRepository<FreelancerProfile>(dbSet), IFreelancerProfileRepository
{
    private readonly DbSet<FreelancerProfile> _profiles = dbSet;

    // Returns null when no profile exists: callers decide whether that is an error. (It used to throw, which made
    // creating a freelancer profile impossible, because the create handler checks for an existing profile first.)
    public Task<FreelancerProfile?> GetLoggedInFreelancerProfileAsync(Guid accountId,
        CancellationToken cancellationToken) =>
        _profiles
            .AsNoTracking()
            .Include(f => f.ForeignLanguages)
            .Include(f => f.Skills)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.AccountId == accountId, cancellationToken);

    public Task<FreelancerProfile?> GetByIdWithTrackingAsync(Guid id, CancellationToken cancellationToken = default,
        params Expression<Func<FreelancerProfile, object>>[] includes)
    {
        IQueryable<FreelancerProfile> query = _profiles;
        foreach (var include in includes ?? [])
            query = query.Include(include);

        return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
