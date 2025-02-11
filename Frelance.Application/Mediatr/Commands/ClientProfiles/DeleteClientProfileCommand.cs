using MediatR;

namespace Frelance.Application.Mediatr.Commands.ClientProfiles;

public record DeleteClientProfileCommand(int Id): IRequest<Unit>;