using Freelance.Contracts.Enums;
using JetBrains.Annotations;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.Users;

[UsedImplicitly]
public record DeleteCurrentAccountCommand(Role Role) : IRequest;