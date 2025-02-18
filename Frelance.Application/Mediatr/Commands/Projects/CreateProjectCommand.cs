using Frelance.Contracts.Requests.Projects;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Projects;

public record CreateProjectCommand(CreateProjectRequest CreateProjectRequest):IRequest<Unit>;