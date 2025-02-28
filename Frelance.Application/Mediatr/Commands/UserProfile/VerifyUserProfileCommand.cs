using Frelance.Contracts.Enums;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.UserProfile;

public record VerifyUserProfileCommand(int Id, Role Role) : IRequest;