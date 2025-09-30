using MediatR;

namespace Freelance.Application.Mediatr.Commands.Projects;

public record DeleteProjectCommand(int Id) : IRequest;