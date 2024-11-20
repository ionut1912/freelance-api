using MediatR;

namespace Frelance.Application.Commands.TimeLogs.CreateTimeLog;

public record CreateTimeLogCommand(string TaskTitle,DateTime StartTime,DateTime EndTime,DateOnly Date):IRequest<int>;