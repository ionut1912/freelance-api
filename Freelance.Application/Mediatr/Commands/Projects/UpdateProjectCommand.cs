using Freelance.Contracts.Requests.Projects;
using JetBrains.Annotations;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.Projects;

[UsedImplicitly]
public record UpdateProjectCommand(int Id, UpdateProjectRequest UpdateProjectRequest) : IRequest;