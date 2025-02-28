using Frelance.Contracts.Enums;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.UserProfile;

public record CreateUserProfileCommand(Role Role,object CreateProfileRequest) : IRequest<Unit>;