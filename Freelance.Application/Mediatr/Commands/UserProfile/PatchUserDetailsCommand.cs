using Freelance.Contracts.Dtos;
using Freelance.Contracts.Enums;
using JetBrains.Annotations;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.UserProfile;

[UsedImplicitly]
public record PatchUserDetailsCommand(Role Role, int Id, UserDetailsDto UserDetails) : IRequest;