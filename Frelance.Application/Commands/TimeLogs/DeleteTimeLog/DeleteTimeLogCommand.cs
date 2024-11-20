using MediatR;

namespace Frelance.Application.Commands.TimeLogs.DeleteTimeLog;

public record DeleteTimeLogCommand(int Id):IRequest<Unit>;