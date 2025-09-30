using Freelance.Contracts.Requests.Reviews;
using JetBrains.Annotations;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.Reviews;

[UsedImplicitly]
public record CreateReviewCommand(CreateReviewRequest CreateReviewRequest) : IRequest;