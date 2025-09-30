using Freelance.Application.Mediatr.Commands.Tasks;
using Freelance.Application.Mediatr.Queries.Tasks;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Responses.Common;

namespace Freelance.Application.Repositories;

public interface ITaskRepository
{
    Task CreateTaskAsync(CreateTaskCommand createTaskCommand, CancellationToken cancellationToken);
    Task UpdateTaskAsync(UpdateTaskCommand updateTaskCommand, CancellationToken cancellationToken);
    Task DeleteTaskAsync(DeleteTaskCommand deleteTaskCommand, CancellationToken cancellationToken);
    Task<TaskDto> GetTaskByIdAsync(GetTaskByIdQuery getTaskByIdQuery, CancellationToken cancellationToken);
    Task<PaginatedList<TaskDto>> GetTasksAsync(GetTasksQuery getTasksQuery, CancellationToken cancellationToken);
}