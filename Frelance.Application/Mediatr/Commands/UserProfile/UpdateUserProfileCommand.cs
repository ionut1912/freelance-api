using Frelance.Contracts.Enums;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.UserProfile;

public record UpdateUserProfileCommand(int Id, Role Role, object UpdateProfileRequest) : IRequest;