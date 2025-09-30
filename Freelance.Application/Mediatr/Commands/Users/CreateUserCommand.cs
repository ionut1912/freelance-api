using Freelance.Contracts.Dtos;
using JetBrains.Annotations;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.Users;

[UsedImplicitly]
public record CreateUserCommand(RegisterDto RegisterDto) : IRequest;