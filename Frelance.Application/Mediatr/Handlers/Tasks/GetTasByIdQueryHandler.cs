using Frelance.Application.Mediatr.Queries.Tasks;
using Frelance.Application.Repositories;
using Frelance.Contracts.Responses.Tasks;
using MediatR;


namespace Frelance.Application.Mediatr.Handlers.Tasks;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, GetTaskByIdResponse>
{
    private readonly ITaskRepository _taskRepository;

    public GetTaskByIdQueryHandler(ITaskRepository taskRepository)
    {
        ArgumentNullException.ThrowIfNull(taskRepository, nameof(taskRepository));
        _taskRepository = taskRepository;
    }
    public async Task<GetTaskByIdResponse> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        return await _taskRepository.GetTaskByIdAsync(request, cancellationToken);
    }
}