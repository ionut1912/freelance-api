using Frelance.API.Frelance.Contracts.Dtos;

namespace Frelance.API.Frelance.Contracts.Responses;

public record GetProjectsResponse(PaginatedList<ProjectDto> Results);