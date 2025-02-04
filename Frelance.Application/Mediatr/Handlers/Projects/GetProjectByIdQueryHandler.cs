using Frelance.Application.Mediatr.Queries.Projects;
using Frelance.Application.Repositories;
using Frelance.Contracts.Responses.Projects;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Projects;

public class GetProjectByIdQueryHandler:IRequestHandler<GetProjectByIdQuery,GetProjectByIdResponse>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectByIdQueryHandler(IProjectRepository projectRepository)
    {
        ArgumentNullException.ThrowIfNull(projectRepository, nameof(projectRepository));
        _projectRepository = projectRepository;
    }
    public async Task<GetProjectByIdResponse> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        return await _projectRepository.FindProjectByIdAsync(request,cancellationToken);
    }
}