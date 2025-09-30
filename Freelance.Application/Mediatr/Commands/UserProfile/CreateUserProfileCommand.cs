using Freelance.Contracts.Enums;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.UserProfile;

public record CreateUserProfileCommand(Role Role, object CreateProfileRequest) : IRequest;