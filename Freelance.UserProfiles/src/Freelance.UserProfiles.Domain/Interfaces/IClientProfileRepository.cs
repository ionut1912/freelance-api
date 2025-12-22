using Freelance.UserProfiles.Domain.Entities;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Domain.Interfaces;

public interface IClientProfileRepository : IGenericRepository<ClientProfile>
{
    Task<ClientProfile> GetLoggedInFreelancerProfileAsync(Guid accountId, CancellationToken cancellationToken);
}