using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.ValueObjects;

namespace Freelance.UserProfiles.Domain.Interfaces;

public interface IClientProfileRepository
{    Task CreateClientProfileAsync(ClientProfile clientProfile,CancellationToken cancellationToken);
     Task UpdateClientProfileAsync(ClientProfile clientProfile, CancellationToken cancellationToken);
     Task DeleteClientProfileAsync(ClientProfile clientProfile, CancellationToken cancellationToken);
    Task<ClientProfile> GetClientProfileByIdAsync(Guid id,CancellationToken cancellationToken);
    Task<List<ClientProfile>> GetClientProfilesAsync(CancellationToken cancellationToken);
    Task<ClientProfile> GetLoggedInFreelancerProfileAsync(Guid accountId,CancellationToken cancellationToken);
}