using Frelance.Application.Mediatr.Commands.Reviews;
using Frelance.Application.Mediatr.Queries.Reviews;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Requests.Reviews;
using Frelance.Web.Extensions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Frelance.Web.Modules;

public static class ReviewsModule
{
    public static void AddReviewsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/reviews", async (IMediator mediator, CreateReviewRequest addReviewRequest,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(addReviewRequest.Adapt<CreateReviewCommand>(), ct);
                return Results.Ok(result);
            }).WithTags("Reviews")
            .RequireAuthorization();
        app.MapGet("/api/reviews/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var project = await mediator.Send(new GetReviewByIdQuery(id), ct);
                return Results.Ok(project);
            }).WithTags("Reviews")
            .RequireAuthorization();

        app.MapGet("/api/reviews",
                async (IMediator mediator, [FromQuery] int pageSize, [FromQuery] int pageNumber,
                    CancellationToken ct) =>
                {
                    var paginatedReviews = await mediator.Send(new GetReviewsQuery
                        (new PaginationParams { PageSize = pageSize, PageNumber = pageNumber }), ct);
                    return Results.Extensions.OkPaginationResult(paginatedReviews.PageSize,
                        paginatedReviews.CurrentPage,
                        paginatedReviews.TotalCount, paginatedReviews.TotalPages, paginatedReviews.Items);
                }).WithTags("Reviews")
            .RequireAuthorization();
        app.MapPut("/api/reviews/{id}", async (IMediator mediator, int id,
                UpdateReviewRequest updateReviewRequest, CancellationToken ct) =>
            {
                var command = updateReviewRequest.Adapt<UpdateReviewCommand>() with { Id = id };
                var result = await mediator.Send(command, ct);
                return Results.Ok(result);
            }).WithTags("Reviews")
            .RequireAuthorization();
        app.MapDelete("/api/reviews/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var command = new DeleteReviewCommand(id);
                var result = await mediator.Send(command, ct);
                return Results.Ok(result);
            }).WithTags("Reviews")
            .RequireAuthorization();
    }
}