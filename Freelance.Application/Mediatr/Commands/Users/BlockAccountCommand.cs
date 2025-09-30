using MediatR;

namespace Freelance.Application.Mediatr.Commands.Users;

public record BlockAccountCommand(string UserId) : IRequest;