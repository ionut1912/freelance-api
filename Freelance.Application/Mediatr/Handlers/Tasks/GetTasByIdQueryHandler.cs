using Freelance.Application.Mediatr.Queries.Tasks;
using Freelance.Application.Repositories;
using Freelance.Contracts.Dtos;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.Tasks;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
{
    private readonly ITaskRepository _taskRepository;

    public GetTaskByIdQueryHandler(ITaskRepository taskRepository)
    {
        ArgumentNullException.ThrowIfNull(taskRepository, nameof(taskRepository));
        _taskRepository = taskRepository;
    }

    public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        return await _taskRepository.GetTaskByIdAsync(request, cancellationToken);
    }
}