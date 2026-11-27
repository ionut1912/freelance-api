using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Queries;
using Freelance.ProjectManagement.Application.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Api.Endpoints;
using Shared.Api.Extensions;
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

    private static async Task<Results<Created<ProjectDto>, UnauthorizedHttpResult>> CreateProject(
        CreateProjectRequest request, HttpContext httpContext, IMessageBus bus, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return TypedResults.Unauthorized();

        var created = await bus.InvokeAsync<ProjectDto>(request.ToCreateCommand(accountId), ct);
        return TypedResults.Created($"/project/{created.Id}", created);
    }

    private static async Task<Ok<List<ProjectDto>>> GetProjects(IMessageBus bus, CancellationToken ct) =>
        TypedResults.Ok(await bus.InvokeAsync<List<ProjectDto>>(new GetAllProjectsQuery(), ct));

    private static async Task<Ok<ProjectDto>> GetProjectById(Guid id, IMessageBus bus, CancellationToken ct) =>
        TypedResults.Ok(await bus.InvokeAsync<ProjectDto>(new GetProjectByIdQuery(id), ct));

    private static async Task<Results<NoContent, UnauthorizedHttpResult>> UpdateProject(Guid id, UpdateProjectRequest request,
        HttpContext httpContext, IMessageBus bus, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return TypedResults.Unauthorized();

        await bus.InvokeAsync(request.ToUpdateCommand(id, accountId), ct);
        return TypedResults.NoContent();
    }

    private static async Task<Results<NoContent, UnauthorizedHttpResult>> DeleteProject(Guid id,
        HttpContext httpContext, IMessageBus bus, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return TypedResults.Unauthorized();

        await bus.InvokeAsync(new DeleteProjectCommand(id, accountId), ct);
        return TypedResults.NoContent();
    }
}
