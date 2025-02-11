using Frelance.Contracts.Dtos;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.Projects;

public record GetProjectByIdQuery(int Id) : IRequest<ProjectDto>;