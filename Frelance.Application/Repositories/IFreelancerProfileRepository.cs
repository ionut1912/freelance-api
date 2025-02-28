using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Requests.FreelancerProfiles;
using Frelance.Contracts.Responses.Common;

namespace Frelance.Application.Repositories;

public interface IFreelancerProfileRepository
{
    Task CreateFreelancerProfileAsync(CreateFreelancerProfileRequest createFreelancerProfileRequest,
        CancellationToken cancellationToken);

    Task<FreelancerProfileDto> GetFreelancerProfileByIdAsync(int id, CancellationToken cancellationToken);

    Task<FreelancerProfileDto> GetLoggedInFreelancerProfileAsync(CancellationToken cancellationToken);

    Task<PaginatedList<FreelancerProfileDto>> GetAllFreelancerProfilesAsync(PaginationParams paginationParams, CancellationToken cancellationToken);

    Task UpdateFreelancerProfileAsync(int id, UpdateFreelancerProfileRequest updateFreelancerProfileRequest, CancellationToken cancellationToken);
    Task VerifyProfileAsync(int id, CancellationToken cancellationToken);

    Task DeleteFreelancerProfileAsync(int id, CancellationToken cancellationToken);
}