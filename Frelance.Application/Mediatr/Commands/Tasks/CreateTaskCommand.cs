using Frelance.Contracts.Requests.ProjectTasks;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Tasks;

public record CreateTaskCommand(CreateProjectTaskRequest CreateProjectTaskRequest) : IRequest;