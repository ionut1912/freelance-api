using Frelance.Contracts.Enums;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.UserProfile;

public record DeleteUserProfileCommand(Role Role, int Id) : IRequest;