using Frelance.Contracts.Requests.FreelancerProfiles;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.FreelancerProfiles;

public record UpdateFreelancerProfileCommand(
    int Id,
    UpdateFreelancerProfileRequest UpdateFreelancerProfileRequest) : IRequest<Unit>;