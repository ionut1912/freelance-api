using Frelance.Contracts.Dtos;
using JetBrains.Annotations;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Users;

[UsedImplicitly]
public record CreateUserCommand(RegisterDto RegisterDto) : IRequest;