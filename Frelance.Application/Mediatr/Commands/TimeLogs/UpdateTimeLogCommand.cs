using Frelance.Contracts.Requests.TimeLogs;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.TimeLogs;

public record UpdateTimeLogCommand(int Id, UpdateTimeLogRequest UpdateTimeLogRequest) : IRequest<Unit>;