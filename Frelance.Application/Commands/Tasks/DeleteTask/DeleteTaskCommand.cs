using MediatR;

namespace Frelance.API.Frelance.Application.Commands.Tasks.DeleteTask;

public record DeleteTaskCommand(int Id):IRequest<Unit>;