using MediatR;

namespace Frelance.Application.Mediatr.Commands.Projects;

public record DeleteProjectCommand(int Id) : IRequest<Unit>;