using Freelance.Application.Mediatr.Commands.UserProfile;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Requests.Common;
using Freelance.Contracts.Requests.FreelancerProfiles;
using Freelance.Contracts.Responses.Common;

namespace Freelance.Application.Repositories;

public interface IFreelancerProfileRepository
{
    Task CreateFreelancerProfileAsync(CreateFreelancerProfileRequest createFreelancerProfileRequest,
        CancellationToken cancellationToken);

    Task<FreelancerProfileDto> GetFreelancerProfileByIdAsync(int id, CancellationToken cancellationToken);

    Task<FreelancerProfileDto> GetLoggedInFreelancerProfileAsync(CancellationToken cancellationToken);

    Task<PaginatedList<FreelancerProfileDto>> GetAllFreelancerProfilesAsync(PaginationParams paginationParams,
        CancellationToken cancellationToken);

    Task PatchAddressAsync(PatchAddressCommand patchAddressCommand, CancellationToken cancellationToken);
    Task PatchUserDetailsAsync(PatchUserDetailsCommand patchUserDetailsCommand, CancellationToken cancellationToken);

    Task PatchFreelancerDetailsAsync(PatchFreelancerDataCommand patchFreelancerDataCommand,
        CancellationToken cancellationToken);

    Task VerifyProfileAsync(int id, CancellationToken cancellationToken);

    Task DeleteFreelancerProfileAsync(int id, CancellationToken cancellationToken);
}