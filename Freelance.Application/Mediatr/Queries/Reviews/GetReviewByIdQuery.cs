using Freelance.Contracts.Dtos;
using MediatR;

namespace Freelance.Application.Mediatr.Queries.Reviews;

public record GetReviewByIdQuery(int Id) : IRequest<ReviewsDto>;