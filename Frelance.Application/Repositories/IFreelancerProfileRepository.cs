using Frelance.Application.Mediatr.Commands.FreelancerProfiles;
using Frelance.Application.Mediatr.Queries.FreelancerProfiles;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;

namespace Frelance.Application.Repositories;

public interface IFreelancerProfileRepository
{
    Task AddFreelancerProfileAsync(AddFreelancerProfileCommand addFreelancerProfileCommand, CancellationToken cancellationToken);
    Task<FreelancerProfileDto> GetFreelancerProfileByIdAsync(GetFreelancerProfileByIdQuery getFreelancerProfileByIdQuery, CancellationToken cancellationToken);
    Task<PaginatedList<FreelancerProfileDto>> GetAllFreelancerProfilesAsync(GetFreelancerProfilesQuery getFreelancerProfilesQuery, CancellationToken cancellationToken);
    Task UpdateFreelancerProfileAsync(UpdateFreelancerProfileCommand updateFreelancerProfileCommand, CancellationToken cancellationToken);
    Task DeleteFreelancerProfileAsync(DeleteFreelancerProfileCommand deleteFreelancerProfileCommand, CancellationToken cancellationToken);
}