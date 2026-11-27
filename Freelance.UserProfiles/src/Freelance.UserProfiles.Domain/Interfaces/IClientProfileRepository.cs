using Freelance.UserProfiles.Domain.Entities;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Domain.Interfaces;

public interface IClientProfileRepository : IGenericRepository<ClientProfile>
{
    /// <summary>Returns the profile owned by <paramref name="accountId"/>, or <see langword="null"/>.</summary>
    Task<ClientProfile?> GetLoggedInClientProfileAsync(Guid accountId, CancellationToken cancellationToken);
}
