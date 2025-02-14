using Frelance.Application.Mediatr.Commands.TimeLogs;
using Frelance.Application.Mediatr.Queries.TimeLogs;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Requests.TimeLogs;
using Frelance.Web.Extensions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Frelance.Web.Modules;

public static class TimeLogModule
{
    public static void AddTimeLogsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/timeLogs", async (IMediator mediator, [FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken ct) =>
        {
            var paginatedTimeLogDtos = await mediator.Send(new GetTimeLogsQuery
                (new PaginationParams { PageSize = pageSize, PageNumber = pageNumber }), ct);
            return Results.Extensions.OkPaginationResult(paginatedTimeLogDtos.PageSize, paginatedTimeLogDtos.CurrentPage,
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
                var createTimeLogCommand = new CreateTimeLogCommand(createTimeLogRequest);
            var result = await mediator.Send(createTimeLogCommand, ct);
            return Results.Ok(result);
        }).WithTags("TimeLogs")
            .RequireAuthorization("FreelancerRole");

        app.MapPut("/api/timeLogs/{id}", async (IMediator mediator, int id,
            UpdateTimeLogRequest updateTimeLogRequest, CancellationToken ct) =>
        {
            var command = updateTimeLogRequest.Adapt<UpdateTimeLogCommand>() with { Id = id };
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).WithTags("TimeLogs")
            .RequireAuthorization("FreelancerRole");

        app.MapDelete("/api/timeLog/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
        {
            var command = new DeleteTimeLogCommand(id);
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).WithTags("TimeLogs")
            .RequireAuthorization("FreelancerRole");
    }
}