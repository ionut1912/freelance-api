using Frelance.Application.Mediatr.Commands.Projects;
using Frelance.Application.Mediatr.Queries.Projects;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Requests.Projects;
using Frelance.Web.Extensions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Frelance.Web.Modules;

public static class ProjectModule
{
    public static void AddProjectsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/projects",
                async (IMediator mediator, [FromQuery] int pageSize, [FromQuery] int pageNumber,
                    CancellationToken ct) =>
                {
                    var paginatedProjectDtos = await mediator.Send(new GetProjectsQuery
                        (new PaginationParams { PageSize = pageSize, PageNumber = pageNumber }), ct);
                    return Results.Extensions.OkPaginationResult(paginatedProjectDtos.PageSize,
                        paginatedProjectDtos.CurrentPage,
                        paginatedProjectDtos.TotalCount, paginatedProjectDtos.TotalPages, paginatedProjectDtos.Items);
                }).WithTags("Projects")
            .RequireAuthorization();

        app.MapGet("/api/projects/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var project = await mediator.Send(new GetProjectByIdQuery(id), ct);
                return Results.Ok(project);
            }).WithTags("Projects")
            .RequireAuthorization();

        app.MapPost("/api/projects", async (IMediator mediator, CreateProjectRequest createProjectRequest,
                CancellationToken ct) =>
            {
                await mediator.Send(createProjectRequest.Adapt<CreateProjectCommand>(), ct);
                return Results.Created();
            }).WithTags("Projects")
            .RequireAuthorization("ClientRole");

        app.MapPut("/api/projects/{id:int}", async (IMediator mediator, int id,
                UpdateProjectRequest updateProjectRequest, CancellationToken ct) =>
            {
                var command = updateProjectRequest.Adapt<UpdateProjectCommand>() with { Id = id };
                await mediator.Send(command, ct);
                return Results.NoContent();
            }).WithTags("Projects")
            .RequireAuthorization("ClientRole");

        app.MapDelete("/api/projects/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var command = new DeleteProjectCommand(id);
                await mediator.Send(command, ct);
                return Results.NoContent();
            }).WithTags("Projects")
            .RequireAuthorization("ClientRole");
    }
}