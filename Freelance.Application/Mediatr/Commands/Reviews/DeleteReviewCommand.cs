using MediatR;

namespace Freelance.Application.Mediatr.Commands.Reviews;

public record DeleteReviewCommand(int Id) : IRequest;