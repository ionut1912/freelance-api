using MediatR;

namespace Frelance.Application.Mediatr.Commands.TimeLogs;

public record CreateTimeLogCommand(string TaskTitle, DateTime StartTime, DateTime EndTime, DateOnly Date) : IRequest<Unit>;