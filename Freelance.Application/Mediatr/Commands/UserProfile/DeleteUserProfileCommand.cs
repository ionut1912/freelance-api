using Freelance.Contracts.Enums;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.UserProfile;

public record DeleteUserProfileCommand(Role Role, int Id) : IRequest;