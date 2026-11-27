using System.Linq.Expressions;
using Freelance.UserProfiles.Domain.Entities;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Domain.Interfaces;

public interface IFreelancerProfileRepository : IGenericRepository<FreelancerProfile>
{
    /// <summary>Returns the profile owned by <paramref name="accountId"/>, or <see langword="null"/>.</summary>
    Task<FreelancerProfile?> GetLoggedInFreelancerProfileAsync(Guid accountId, CancellationToken cancellationToken);

    /// <summary>Loads the profile with change tracking enabled, for commands that mutate the aggregate.</summary>
    Task<FreelancerProfile?> GetByIdWithTrackingAsync(Guid id, CancellationToken cancellationToken = default,
        params Expression<Func<FreelancerProfile, object>>[] includes);
}
