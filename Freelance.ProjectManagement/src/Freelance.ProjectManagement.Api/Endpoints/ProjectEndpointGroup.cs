using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Queries;
using Freelance.ProjectManagement.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Shared.Api.Infrastructure;
using Shared.Application.Mediator;

namespace Freelance.ProjectManagement.Api.Endpoints;

public class ProjectEndpointGroup:EndpointGroup
{
    public override void Map(IEndpointRouteBuilder endpoints)
    {
       var group = endpoints.MapGroup(this);

        group.MapPost(CreateProject);
        group.MapGet(GetProjects);
        group.MapGet(GetProjectById, "/{id:guid}");
        group.MapPut(UpdateProject, "/{id:guid}");
        group.MapDelete(DeleteProject, "/{id:guid}");
    }

    [Authorize(Policy = "ClientOnly")]
    public static async Task<IResult> CreateProject(IMediator mediator,CreateProjectRequest createProjectRequest,CancellationToken cancellationToken)
    {
        var command = createProjectRequest.ToCreateCommand();
        var created = await mediator.Send(command, cancellationToken);
        return Results.Created($"/project/{created.Id}", created);
    }

    [Authorize]
    public static async Task<IResult> GetProjects(IMediator mediator, CancellationToken cancellationToken) 
    {
        var query = new GetAllProjectsQuery();
        var projects = await mediator.Send(query, cancellationToken);
        return Results.Ok(projects);
    }

    [Authorize]
    public static async Task<IResult> GetProjectById(IMediator mediator,Guid id,CancellationToken cancellationToken)
    {
        var query = new GetProjectByIdQuery(id);
        var project=await mediator.Send(query, cancellationToken);
        return Results.Ok(project);
    }

    [Authorize(Policy = "ClientOnly")]
    public static async Task<IResult> UpdateProject(IMediator mediator,Guid id, UpdateProjectRequest updateProjectRequest,CancellationToken cancellationToken)
    {
        var comand = updateProjectRequest.ToUpdateCommand(id);
        await mediator.Send(comand, cancellationToken);
        return Results.NoContent();
    }

    [Authorize(Policy = "ClientOnly")]
    public static async Task<IResult> DeleteProject(IMediator mediator,Guid id,CancellationToken cancellationToken)
    {
        var command = new DeleteProjectCommand(id);
        await mediator.Send(command, cancellationToken);
        return Results.NoContent();
    }
}
