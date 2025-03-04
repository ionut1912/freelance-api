using MediatR;

namespace Frelance.Application.Mediatr.Commands.Users;

public record DeleteAccountCommand(string UserId) : IRequest;