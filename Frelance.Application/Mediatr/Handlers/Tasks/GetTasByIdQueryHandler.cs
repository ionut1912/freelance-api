using Frelance.Application.Mediatr.Queries.Tasks;
using Frelance.Application.Repositories;
using Frelance.Contracts.Responses.Tasks;
using MediatR;


namespace Frelance.Application.Mediatr.Handlers.Tasks;

public class GetTaskByIdQueryHandler:IRequestHandler<GetTaskByIdQuery,GetTaskByIdResponse>
{
    private readonly ITaskRepository _repository;

    public GetTaskByIdQueryHandler(ITaskRepository repository)
    {
        _repository = repository;
    }
    public async Task<GetTaskByIdResponse> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetTaskByIdAsync(request,cancellationToken);
    }
}