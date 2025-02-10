using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.Projects;

public record GetProjectsQuery(PaginationParams PaginationParams) : IRequest<PaginatedList<ProjectDto>>;