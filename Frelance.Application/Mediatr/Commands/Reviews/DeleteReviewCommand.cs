using MediatR;

namespace Frelance.Application.Mediatr.Commands.Reviews;

public record DeleteReviewCommand(int Id) : IRequest<Unit>;