using MediatR;

namespace Frelance.Application.Mediatr.Commands.Reviews;

public record AddReviewCommand(string ReviewText) : IRequest<Unit>;