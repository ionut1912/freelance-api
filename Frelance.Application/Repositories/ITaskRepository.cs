using Frelance.Application.Mediatr.Commands.Tasks;
using Frelance.Application.Mediatr.Queries.Tasks;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;
using Frelance.Contracts.Responses.Tasks;

namespace Frelance.Application.Repositories;

public interface ITaskRepository
{
    Task AddTaskAsync(CreateTaskCommand createTaskCommand,CancellationToken cancellationToken);
    Task UpdateTaskAsync(UpdateTaskCommand updateTaskCommand,CancellationToken cancellationToken);
    Task DeleteTaskAsync(DeleteTaskCommand deleteTaskCommand,CancellationToken cancellationToken);
    Task<GetTaskByIdResponse> GetTaskByIdAsync(GetTaskByIdQuery getTaskByIdQuery,CancellationToken cancellationToken);
    Task<PaginatedList<TaskDto>> GetTasksAsync(GetTasksQuery getTasksQuery,CancellationToken cancellationToken);  
}