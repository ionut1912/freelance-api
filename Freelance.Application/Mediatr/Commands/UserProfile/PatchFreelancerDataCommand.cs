using Freelance.Contracts.Dtos;
using JetBrains.Annotations;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.UserProfile;

[UsedImplicitly]
public record PatchFreelancerDataCommand(int Id, FreelancerProfileData FreelancerProfileData) : IRequest;