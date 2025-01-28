using Frelance.Contracts.Dtos;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Users;

public record LoginCommand(LoginDto LoginDto):IRequest<UserDto>;