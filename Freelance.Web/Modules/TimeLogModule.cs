using Freelance.Application.Mediatr.Commands.TimeLogs;
using Freelance.Application.Mediatr.Queries.TimeLogs;
using Freelance.Contracts.Requests.Common;
using Freelance.Contracts.Requests.TimeLogs;
using Freelance.Web.Extensions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Freelance.Web.Modules;

public static class TimeLogModule
{
    public static void AddTimeLogsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/timeLogs",
                async (IMediator mediator, [FromQuery] int pageSize, [FromQuery] int pageNumber,
                    CancellationToken ct) =>
                {
                    var paginatedTimeLogDtos = await mediator.Send(new GetTimeLogsQuery
                        (new PaginationParams { PageSize = pageSize, PageNumber = pageNumber }), ct);
                    return Results.Extensions.OkPaginationResult(paginatedTimeLogDtos.PageSize,
                        paginatedTimeLogDtos.CurrentPage,
                        paginatedTimeLogDtos.TotalCount, paginatedTimeLogDtos.TotalPages, paginatedTimeLogDtos.Items);
                }).WithTags("TimeLogs")
            .RequireAuthorization();

        app.MapGet("/api/timeLogs/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var timeLog = await mediator.Send(new GetTimeLogByIdQuery(id), ct);
                return Results.Ok(timeLog);
            }).WithTags("TimeLogs")
            .RequireAuthorization();

        app.MapPost("/api/timeLogs", async (IMediator mediator, CreateTimeLogRequest createTimeLogRequest,
                CancellationToken ct) =>
            {
                await mediator.Send(createTimeLogRequest.Adapt<CreateTimeLogCommand>(), ct);
                return Results.Created();
            }).WithTags("TimeLogs")
            .RequireAuthorization("FreelancerRole");

        app.MapPut("/api/timeLogs/{id}", async (IMediator mediator, int id,
                UpdateTimeLogRequest updateTimeLogRequest, CancellationToken ct) =>
            {
                var command = updateTimeLogRequest.Adapt<UpdateTimeLogCommand>() with { Id = id };
                await mediator.Send(command, ct);
                return Results.NoContent();
            }).WithTags("TimeLogs")
            .RequireAuthorization("FreelancerRole");

        app.MapDelete("/api/timeLog/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var command = new DeleteTimeLogCommand(id);
                await mediator.Send(command, ct);
                return Results.NoContent();
            }).WithTags("TimeLogs")
            .RequireAuthorization("FreelancerRole");
    }
}