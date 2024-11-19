using MediatR;

namespace Frelance.Application.Queries.Projects.GetProjects;

public record GetProjectsQuery(PaginationParams PaginationParams):IRequest<PaginatedList<ProjectDto>>;
