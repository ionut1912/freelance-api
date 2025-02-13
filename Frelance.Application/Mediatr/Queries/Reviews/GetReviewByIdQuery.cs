using Frelance.Contracts.Dtos;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.Reviews;

public record GetReviewByIdQuery(int Id):IRequest<ReviewsDto>;