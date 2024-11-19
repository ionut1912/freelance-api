using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Queries.Projects.GetProjects;

public record GetProjectsQuery(PaginationParams PaginationParams):IRequest<PaginatedList<ProjectDto>>;
