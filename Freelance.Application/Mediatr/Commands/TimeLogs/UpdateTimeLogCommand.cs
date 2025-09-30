using Freelance.Contracts.Requests.TimeLogs;
using JetBrains.Annotations;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.TimeLogs;

[UsedImplicitly]
public record UpdateTimeLogCommand(int Id, UpdateTimeLogRequest UpdateTimeLogRequest) : IRequest;