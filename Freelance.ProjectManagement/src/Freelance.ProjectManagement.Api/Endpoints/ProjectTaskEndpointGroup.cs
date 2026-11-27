using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Querires;
using Freelance.ProjectManagement.Application.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Api.Endpoints;
using Wolverine;

namespace Freelance.ProjectManagement.Api.Endpoints;

public sealed class ProjectTaskEndpointGroup : EndpointGroup
{
    public override void Map(IEndpointRouteBuilder app)
    {
        // Explicit prefix: it is part of the public contract routed by the API gateway (/projectTask/*).
        var group = app.MapGroup("/projectTask").WithTags("Project Task").RequireAuthorization();

        group.MapPost("", CreateProjectTask).WithName(nameof(CreateProjectTask));
        group.MapGet("", GetProjectTasks).WithName(nameof(GetProjectTasks));
        group.MapGet("/{id:guid}", GetProjectTaskById).WithName(nameof(GetProjectTaskById));
        group.MapPut("/{id:guid}", UpdateProjectTask).WithName(nameof(UpdateProjectTask));
        group.MapDelete("/{id:guid}", DeleteProjectTask).WithName(nameof(DeleteProjectTask)).RequireAuthorization(AuthorizationPolicies.ClientOnly);
    }

    private static async Task<Created<ProjectTaskDto>> CreateProjectTask(CreateProjectTaskRequest request,
        IMessageBus bus, CancellationToken ct)
    {
        var created = await bus.InvokeAsync<ProjectTaskDto>(request.ToCreateCommand(), ct);
        return TypedResults.Created($"/projectTask/{created.Id}", created);
    }

    private static async Task<Ok<List<ProjectTaskDto>>> GetProjectTasks(IMessageBus bus, CancellationToken ct) =>
        TypedResults.Ok(await bus.InvokeAsync<List<ProjectTaskDto>>(new GetAllProjectTasksQuery(), ct));

    private static async Task<Ok<ProjectTaskDto>> GetProjectTaskById(Guid id, IMessageBus bus, CancellationToken ct) =>
        TypedResults.Ok(await bus.InvokeAsync<ProjectTaskDto>(new GetProjectTaskByIdQuery(id), ct));

    private static async Task<NoContent> UpdateProjectTask(Guid id, UpdateProjectTaskReuqest request, IMessageBus bus,
        CancellationToken ct)
    {
        await bus.InvokeAsync(request.ToUpdateCommand(id), ct);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> DeleteProjectTask(Guid id, IMessageBus bus, CancellationToken ct)
    {
        await bus.InvokeAsync(new DeleteProjectTaskCommand(id), ct);
        return TypedResults.NoContent();
    }
}
