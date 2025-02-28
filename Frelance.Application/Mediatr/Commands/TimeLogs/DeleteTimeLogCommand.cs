using MediatR;

namespace Frelance.Application.Mediatr.Commands.TimeLogs;

public record DeleteTimeLogCommand(int Id) : IRequest;