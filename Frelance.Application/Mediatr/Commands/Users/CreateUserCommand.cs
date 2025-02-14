using Frelance.Contracts.Dtos;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Users;

public record CreateUserCommand(RegisterDto RegisterDto) : IRequest<Unit>;
