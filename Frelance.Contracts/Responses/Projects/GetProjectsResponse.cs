
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;

namespace Frelance.Contracts.Responses.Projects;

public record GetProjectsResponse(PaginatedList<ProjectDto> Results);