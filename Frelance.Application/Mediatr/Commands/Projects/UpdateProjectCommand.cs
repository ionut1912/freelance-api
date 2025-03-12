using Frelance.Contracts.Requests.Projects;
using JetBrains.Annotations;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Projects;

[UsedImplicitly]
public record UpdateProjectCommand(int Id, UpdateProjectRequest UpdateProjectRequest) : IRequest;
