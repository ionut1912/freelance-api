using Freelance.Contracts.Requests.TimeLogs;
using JetBrains.Annotations;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.TimeLogs;

[UsedImplicitly]
public record CreateTimeLogCommand(CreateTimeLogRequest CreateTimeLogRequest) : IRequest;