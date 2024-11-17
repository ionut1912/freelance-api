
using Frelance.API.Frelamce.Contracts;
using Frelance.API.Frelamce.Contracts.Common;
using MediatR;

namespace Frelance.Application.Queries.Projects.GetProjects;

public record GetProjectsQuery(PaginationParams PaginationParams):IRequest<PaginatedList<ProjectDto>>;