using Frelance.Contracts.Requests.TimeLogs;
using JetBrains.Annotations;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.TimeLogs;

[UsedImplicitly]
public record CreateTimeLogCommand(CreateTimeLogRequest CreateTimeLogRequest) : IRequest;