using MediatR;

namespace Frelance.Application.Mediatr.Commands.Tasks;

public record DeleteTaskCommand(int Id) : IRequest<Unit>;