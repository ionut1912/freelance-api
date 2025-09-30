using Freelance.Application.Mediatr.Queries.Tasks;
using Freelance.Application.Repositories;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Responses.Common;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.Tasks;

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, PaginatedList<TaskDto>>
{
    private readonly ITaskRepository _taskRepository;

    public GetTasksQueryHandler(ITaskRepository taskRepository)
    {
        ArgumentNullException.ThrowIfNull(taskRepository, nameof(taskRepository));
        _taskRepository = taskRepository;
    }

    public async Task<PaginatedList<TaskDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        return await _taskRepository.GetTasksAsync(request, cancellationToken);
    }
}