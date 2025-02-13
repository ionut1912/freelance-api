using MediatR;

namespace Frelance.Application.Mediatr.Commands.Reviews;

public record UpdateReviewCommand(int Id,string ReviewText): IRequest<Unit>;
