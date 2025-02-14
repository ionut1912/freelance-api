using Frelance.Contracts.Requests.FreelancerProfiles;
using Frelance.Contracts.Requests.Skills;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Frelance.Application.Mediatr.Commands.FreelancerProfiles;

public record UpdateFreelancerProfileCommand(
    int Id,
   UpdateFreelancerProfileRequest UpdateFreelancerProfileRequest) : IRequest<Unit>;