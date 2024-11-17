
using Frelamce.Contracts.Requests.ProjectTasks;
using Frelance.API.Extensions;
using Frelance.API.Frelamce.Contracts.Common;
using Frelance.API.Frelamce.Contracts.Projects;
using Frelance.API.Frelance.Application.Commands.Projects.CreateProject;
using Frelance.API.Frelance.Application.Commands.Tasks.CreateTask;
using Frelance.API.Frelance.Application.Commands.Tasks.DeleteTask;
using Frelance.API.Frelance.Application.Commands.Tasks.UpdateTask;
using Frelance.API.Frelance.Application.Queries.Projects.GetProjectById;
using Frelance.API.Frelance.Application.Queries.Tasks.GetTaskById;
using Frelance.API.Frelance.Application.Queries.Tasks.GetTasks;
using Frelance.Application.Commands.Projects.DeleteProject;
using Frelance.Application.Commands.Projects.UpdateProject;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Frelance.API.Modules;

public static class TaskModule
{
      public static void AddTasksEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/tasks", async (IMediator mediator, [FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken ct) =>
            {
                var paginatedProjectDtos = await mediator.Send(new GetTasksQuery
                    (new PaginationParams {PageSize = pageSize, PageNumber = pageNumber}), ct);
                return Results.Extensions.OkPaginationResult(paginatedProjectDtos.PageSize, paginatedProjectDtos.CurrentPage,
                    paginatedProjectDtos.TotalCount, paginatedProjectDtos.TotalPages, paginatedProjectDtos.Items);
            }).WithTags("Tasks");
    
            app.MapGet("/api/tasks/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var task = await mediator.Send(new GetTaskByIdQuery(id), ct);
                return Results.Ok(task);
            }).WithTags("Tasks");
    
            app.MapPost("/api/tasks", async (IMediator mediator, CreateProjectTaskRequest createProjectTaskRequest,
                CancellationToken ct) =>
            {
                var command = createProjectTaskRequest.Adapt<CreateTaskCommand>();
                var result = await mediator.Send(command, ct);
                return Results.Ok(result);
            }).WithTags("Tasks");
    
            app.MapPut("/api/tasks/{id}", async (IMediator mediator, int id,
                UpdateProjectTaskRequest updateTaskRequest, CancellationToken ct) =>
            {
                var command = updateTaskRequest.Adapt<UpdateTaskCommand>() with { Id = id };
                var result = await mediator.Send(command, ct);
                return Results.Ok(result);
            }).WithTags("Tasks");
    
            app.MapDelete("/api/task/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var command = new DeleteTaskCommand(id);
                var result = await mediator.Send(command, ct);
                return Results.Ok(result);
            }).WithTags("Tasks");
        }
}