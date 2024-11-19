using MediatR;

namespace Frelance.Application.Commands.Tasks.CreateTask;

public record CreateTaskCommand(string ProjectTitle,string Title,string Description,Priority Priority) : IRequest<int>;