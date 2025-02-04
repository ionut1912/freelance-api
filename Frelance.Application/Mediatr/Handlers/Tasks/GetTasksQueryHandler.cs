using Frelance.Application.Helpers;
using Frelance.Application.Mediatr.Queries.Tasks;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Tasks;

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
        return await _taskRepository.GetTasksAsync(request,cancellationToken);
    }
}