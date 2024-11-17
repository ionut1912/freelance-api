using MediatR;

namespace Frelance.Application.Commands.Projects.DeleteProject;

public record  DeleteProjectCommand(int Id) : IRequest<Unit>;