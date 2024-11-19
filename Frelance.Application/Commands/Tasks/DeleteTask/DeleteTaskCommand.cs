using MediatR;

namespace Frelance.Application.Commands.Tasks.DeleteTask;

public record DeleteTaskCommand(int Id):IRequest<Unit>;