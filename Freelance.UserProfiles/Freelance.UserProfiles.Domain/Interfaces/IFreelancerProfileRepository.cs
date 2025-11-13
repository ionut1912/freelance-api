using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.ValueObjects;

namespace Freelance.UserProfiles.Domain.Interfaces;

public interface IFreelancerProfileRepository
{
    Task CreateFreelancerProfileAsync(FreelancerProfile createFreelancerProfileRequest, CancellationToken cancellationToken);
    Task UpdateFreelancerProfileAsync(FreelancerProfile freelancerProfile, CancellationToken cancellationToken);
    Task UpdateFreelancerProfileDetails(FreelancerProfile freelancerProfile,List<Skill> skills, CancellationToken cancellationToken);
    Task DeleteFreelancerProfileAsync(FreelancerProfile freelancerProfile, CancellationToken cancellationToken);
    Task<FreelancerProfile> GetFreelancerProfileByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<FreelancerProfile> GetLoggedInFreelancerProfileAsync(Guid accountId,CancellationToken cancellationToken);

    Task<List<FreelancerProfile>> GetFreelancerProfilesAsync(CancellationToken cancellationToken);
}