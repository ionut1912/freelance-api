using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Querires;
using Freelance.ProjectManagement.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Shared.Api.Infrastructure;
using Shared.Application.Mediator;

namespace Freelance.ProjectManagement.Api.Endpoints;

public class ProjectTaskEndpointGroup:EndpointGroup
{
    public override void Map(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(this);

        group.MapPost(CreaterProjectTask);
        group.MapGet(GetProjectTasks);
        group.MapGet(GetProjectTaskById, "/{id:guid}");
        group.MapPut(UpdateProjectTask, "/{id:guid}");
        group.MapDelete(DeleteProjectTask, "/{id:guid}");
    }

    [Authorize]
    public static async Task<IResult> CreaterProjectTask(IMediator mediator, CreateProjectTaskRequest createProjectTaskRequest, CancellationToken cancellationToken)
    {
        var command = createProjectTaskRequest.ToCreateCommand();
        var created = await mediator.Send(command, cancellationToken);
        return Results.Created($"/projectTask/{created.Id}", created);
    }

    [Authorize]
    public static async Task<IResult> GetProjectTasks(IMediator mediator, CancellationToken cancellationToken)
    {
        var query = new GetAllProjectTasksQuery();
        var projectTasks = await mediator.Send(query, cancellationToken);
        return Results.Ok(projectTasks);
    }

    [Authorize]
    public static async Task<IResult> GetProjectTaskById(IMediator mediator, Guid id, CancellationToken cancellationToken)
    {
        var query = new GetProjectTaskByIdQuery(id);
        var projectTask = await mediator.Send(query, cancellationToken);
        return Results.Ok(projectTask);
    }

    [Authorize]
    public static async Task<IResult> UpdateProjectTask(IMediator mediator, Guid id, UpdateProjectTaskReuqest updateProjectTaskReuqest, CancellationToken cancellationToken)
    {
        var comand = updateProjectTaskReuqest.ToUpdateCommand(id);
        await mediator.Send(comand, cancellationToken);
        return Results.NoContent();
    }

    [Authorize(Policy = "ClientOnly")]
    public static async Task<IResult> DeleteProjectTask(IMediator mediator, Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteProjectTaskCommand(id);
        await mediator.Send(command, cancellationToken);
        return Results.NoContent();
    }
}
