using Frelance.Contracts.Requests.Address;
using Frelance.Contracts.Requests.FreelancerProfiles;
using Frelance.Contracts.Requests.Skills;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Frelance.Application.Mediatr.Commands.FreelancerProfiles;

public record CreateFreelancerProfileCommand(
CreateFreelancerProfieRequest CreateFreelancerProfileRequest) : IRequest<Unit>;
