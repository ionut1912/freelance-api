using Freelance.ProjectManagement.Application.Dtos;
using Shared.Application.Mediator;

namespace Freelance.ProjectManagement.Application.Mediatr.Projects.Queries;

public record GetProjectByIdQuery(Guid Id) : IRequest<ProjectDto>
{
}
