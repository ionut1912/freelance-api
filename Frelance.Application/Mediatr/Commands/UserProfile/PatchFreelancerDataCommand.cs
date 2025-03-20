using Frelance.Contracts.Dtos;
using JetBrains.Annotations;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.UserProfile;

[UsedImplicitly]
public record PatchFreelancerDataCommand(int Id, FreelancerProfileData FreelancerProfileData) : IRequest;