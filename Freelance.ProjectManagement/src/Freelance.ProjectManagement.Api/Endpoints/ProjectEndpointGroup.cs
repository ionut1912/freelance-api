using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Queries;
using Freelance.ProjectManagement.Application.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Api.Endpoints;
using Wolverine;

namespace Freelance.ProjectManagement.Api.Endpoints;

public sealed class ProjectEndpointGroup : EndpointGroup
{
    public override void Map(IEndpointRouteBuilder app)
    {
        // Explicit prefix: it is part of the public contract routed by the API gateway (/project/*).
        var group = app.MapGroup("/project").WithTags("Project").RequireAuthorization();

        group.MapPost("", CreateProject).WithName(nameof(CreateProject)).RequireAuthorization(AuthorizationPolicies.ClientOnly);
        group.MapGet("", GetProjects).WithName(nameof(GetProjects));
        group.MapGet("/{id:guid}", GetProjectById).WithName(nameof(GetProjectById));
        group.MapPut("/{id:guid}", UpdateProject).WithName(nameof(UpdateProject)).RequireAuthorization(AuthorizationPolicies.ClientOnly);
        group.MapDelete("/{id:guid}", DeleteProject).WithName(nameof(DeleteProject)).RequireAuthorization(AuthorizationPolicies.ClientOnly);
    }

    private static async Task<Created<ProjectDto>> CreateProject(CreateProjectRequest request, IMessageBus bus,
        CancellationToken ct)
    {
        var created = await bus.InvokeAsync<ProjectDto>(request.ToCreateCommand(), ct);
        return TypedResults.Created($"/project/{created.Id}", created);
    }

    private static async Task<Ok<List<ProjectDto>>> GetProjects(IMessageBus bus, CancellationToken ct) =>
        TypedResults.Ok(await bus.InvokeAsync<List<ProjectDto>>(new GetAllProjectsQuery(), ct));

    private static async Task<Ok<ProjectDto>> GetProjectById(Guid id, IMessageBus bus, CancellationToken ct) =>
        TypedResults.Ok(await bus.InvokeAsync<ProjectDto>(new GetProjectByIdQuery(id), ct));

    private static async Task<NoContent> UpdateProject(Guid id, UpdateProjectRequest request, IMessageBus bus,
        CancellationToken ct)
    {
        await bus.InvokeAsync(request.ToUpdateCommand(id), ct);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> DeleteProject(Guid id, IMessageBus bus, CancellationToken ct)
    {
        await bus.InvokeAsync(new DeleteProjectCommand(id), ct);
        return TypedResults.NoContent();
    }
}
