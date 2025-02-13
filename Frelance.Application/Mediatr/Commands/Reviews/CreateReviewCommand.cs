using Frelance.Contracts.Requests.Reviews;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Reviews;

public record CreateReviewCommand(CreateReviewRequest CreateReviewRequest) : IRequest<Unit>;