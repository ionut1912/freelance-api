using Freelance.Contracts.Dtos;
using MediatR;

namespace Freelance.Application.Mediatr.Queries.Projects;

public record GetProjectByIdQuery(int Id) : IRequest<ProjectDto>;