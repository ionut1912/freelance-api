

using MediatR;

namespace Frelance.API.Frelance.Application.Commands.Projects.DeleteProject;

public record  DeleteProjectCommand(int Id) : IRequest<Unit>;