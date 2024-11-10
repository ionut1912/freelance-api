using Frelance.API.Frelance.Contracts.Dtos;
using Frelance.API.Frelance.Contracts.Requests.Common;
using Frelance.API.Frelance.Contracts.Responses;
using MediatR;

namespace Frelance.API.Frelance.Application.Queries.Projects.GetProjects;

public record GetProjectsQuery(PaginationParams PaginationParams):IRequest<PaginatedList<ProjectDto>>;