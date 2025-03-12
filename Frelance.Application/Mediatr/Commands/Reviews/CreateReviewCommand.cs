using Frelance.Contracts.Requests.Reviews;
using JetBrains.Annotations;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Reviews;

[UsedImplicitly]
public record CreateReviewCommand(CreateReviewRequest CreateReviewRequest) : IRequest;