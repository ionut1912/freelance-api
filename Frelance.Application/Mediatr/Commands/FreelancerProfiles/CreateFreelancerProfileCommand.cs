using Frelance.Contracts.Requests.FreelancerProfiles;
using Frelance.Contracts.Requests.Skills;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Frelance.Application.Mediatr.Commands.FreelancerProfiles;

public record CreateFreelancerProfileCommand(
CreateFreelancerProfileRequest CreateFreelancerProfileRequest) : IRequest<Unit>;
