using MediatR;
using Frelance.Contracts.Responses.Common;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Dtos;
namespace Frelance.Application.Queries.Projects.GetProjects;

public record GetProjectsQuery(PaginationParams PaginationParams):IRequest<PaginatedList<ProjectDto>>;
