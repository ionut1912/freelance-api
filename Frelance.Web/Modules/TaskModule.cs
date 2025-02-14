using Frelance.Application.Mediatr.Commands.Tasks;
using Frelance.Application.Mediatr.Queries.Tasks;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Requests.ProjectTasks;
using Frelance.Web.Extensions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Frelance.Web.Modules;

public static class TaskModule
{
    public static void AddTasksEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tasks", async (IMediator mediator, [FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken ct) =>
        {
            var paginatedProjectDtos = await mediator.Send(new GetTasksQuery
                (new PaginationParams { PageSize = pageSize, PageNumber = pageNumber }), ct);
            return Results.Extensions.OkPaginationResult(paginatedProjectDtos.PageSize, paginatedProjectDtos.CurrentPage,
                paginatedProjectDtos.TotalCount, paginatedProjectDtos.TotalPages, paginatedProjectDtos.Items);
        }).WithTags("Tasks")
        .RequireAuthorization();

        app.MapGet("/api/tasks/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
        {
            var task = await mediator.Send(new GetTaskByIdQuery(id), ct);
            return Results.Ok(task);
        }).WithTags("Tasks")
        .RequireAuthorization();

        app.MapPost("/api/tasks", async (IMediator mediator, CreateProjectTaskRequest createProjectTaskRequest,
            CancellationToken ct) =>
            {
                var command = new CreateTaskCommand(createProjectTaskRequest);
                var result = await mediator.Send(command, ct);
                return Results.Ok(result);
            }).WithTags("Tasks")
            .RequireAuthorization();

        app.MapPut("/api/tasks/{id}", async (IMediator mediator, int id,
            UpdateProjectTaskRequest updateTaskRequest, CancellationToken ct) =>
        {
            var command = updateTaskRequest.Adapt<UpdateTaskCommand>() with { Id = id };
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).WithTags("Tasks")
            .RequireAuthorization();

        app.MapDelete("/api/task/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
        {
            var command = new DeleteTaskCommand(id);
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).WithTags("Tasks")
            .RequireAuthorization("ClientRole");
    }
}