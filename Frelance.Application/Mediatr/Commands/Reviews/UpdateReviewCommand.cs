using Frelance.Contracts.Requests.Reviews;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Reviews;

public record UpdateReviewCommand(int Id, UpdateReviewRequest UpdateReviewRequest) : IRequest;