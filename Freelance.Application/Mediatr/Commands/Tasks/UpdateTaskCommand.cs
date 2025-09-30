using Freelance.Contracts.Requests.ProjectTasks;
using JetBrains.Annotations;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.Tasks;

[UsedImplicitly]
public record UpdateTaskCommand(int Id, UpdateProjectTaskRequest UpdateProjectTaskRequest) : IRequest;