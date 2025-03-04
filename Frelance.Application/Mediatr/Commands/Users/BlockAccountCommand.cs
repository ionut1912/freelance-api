using MediatR;

namespace Frelance.Application.Mediatr.Commands.Users;

public record BlockAccountCommand(string UserId) : IRequest;