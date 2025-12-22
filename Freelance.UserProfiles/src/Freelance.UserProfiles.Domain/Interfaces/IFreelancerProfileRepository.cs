using Freelance.UserProfiles.Domain.Entities;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Domain.Interfaces;

public interface IFreelancerProfileRepository : IGenericRepository<FreelancerProfile>
{
    Task<FreelancerProfile> GetLoggedInFreelancerProfileAsync(Guid accountId, CancellationToken cancellationToken);

}