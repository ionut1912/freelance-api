using Frelance.Contracts.Requests.FreelancerProfiles;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.FreelancerProfiles;

public record CreateFreelancerProfileCommand(
    CreateFreelancerProfileRequest CreateFreelancerProfileRequest) : IRequest<Unit>;