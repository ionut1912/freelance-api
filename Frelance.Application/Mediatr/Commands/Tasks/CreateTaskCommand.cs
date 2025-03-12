using Frelance.Contracts.Requests.ProjectTasks;
using JetBrains.Annotations;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Tasks;

[UsedImplicitly]
public record CreateTaskCommand(CreateProjectTaskRequest CreateProjectTaskRequest) : IRequest;