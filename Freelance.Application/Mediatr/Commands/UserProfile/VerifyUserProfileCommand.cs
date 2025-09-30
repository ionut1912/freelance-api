using Freelance.Contracts.Enums;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.UserProfile;

public record VerifyUserProfileCommand(int Id, Role Role) : IRequest;