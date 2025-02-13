using Frelance.Contracts.Requests.TimeLogs;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.TimeLogs;

public record CreateTimeLogCommand(CreateTimeLogRequest CreateTimeLogRequest) : IRequest<Unit>;