using Freelance.Contracts.Requests.ProjectTasks;
using JetBrains.Annotations;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.Tasks;

[UsedImplicitly]
public record CreateTaskCommand(CreateProjectTaskRequest CreateProjectTaskRequest) : IRequest;