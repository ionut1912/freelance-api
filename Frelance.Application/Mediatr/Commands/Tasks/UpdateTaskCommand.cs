using Frelance.Contracts.Requests.ProjectTasks;
using JetBrains.Annotations;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Tasks;

[UsedImplicitly]
public record UpdateTaskCommand(int Id, UpdateProjectTaskRequest UpdateProjectTaskRequest) : IRequest;