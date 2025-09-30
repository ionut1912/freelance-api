using Freelance.Contracts.Dtos;
using Freelance.Contracts.Requests.Common;
using Freelance.Contracts.Responses.Common;
using MediatR;

namespace Freelance.Application.Mediatr.Queries.Projects;

public record GetProjectsQuery(PaginationParams PaginationParams) : IRequest<PaginatedList<ProjectDto>>;