using Frelance.Contracts.Requests.Projects;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Projects;

public record UpdateProjectCommand(int Id, UpdateProjectRequest UpdateProjectRequest) : IRequest<Unit>;
