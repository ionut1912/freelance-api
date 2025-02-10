using MediatR;

namespace Frelance.Application.Mediatr.Commands.TimeLogs;

public record UpdateTimeLogCommand(int Id, string TaskTitle, DateTime StartTime, DateTime EndTime, DateOnly Date, int TotalHours) : IRequest<Unit>;