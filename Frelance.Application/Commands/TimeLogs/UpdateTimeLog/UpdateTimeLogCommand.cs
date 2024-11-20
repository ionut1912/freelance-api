using MediatR;

namespace Frelance.Application.Commands.TimeLogs.UpdateTimeLog;

public record UpdateTimeLogCommand(int Id,string TaskTitle,DateTime StartTime,DateTime EndTime,DateOnly Date,int TotalHours):IRequest<Unit>;