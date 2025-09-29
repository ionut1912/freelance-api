using Frelance.Contracts.Enums;
using JetBrains.Annotations;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Users;

[UsedImplicitly]
public record DeleteCurrentAccountCommand(Role Role) : IRequest;
