using Shared.Api.Extensions;
using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Queries;
using Freelance.ProjectManagement.Application.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Api.Endpoints;
using Wolverine;

namespace Freelance.ProjectManagement.Api.Endpoints;

public sealed class TimeLogEndpointGroup : EndpointGroup
{
    public override void Map(IEndpointRouteBuilder app)
    {
        // Explicit prefix: it is part of the public contract routed by the API gateway (/timeLog/*).
        var group = app.MapGroup("/timeLog").WithTags("Time Log").RequireAuthorization();

        group.MapPost("", CreateTimeLog).WithName(nameof(CreateTimeLog)).RequireAuthorization(AuthorizationPolicies.FreelancerOnly);
        group.MapGet("", GetTimeLogs).WithName(nameof(GetTimeLogs));
        group.MapGet("/{id:guid}", GetTimeLogById).WithName(nameof(GetTimeLogById));
        group.MapPut("/{id:guid}", UpdateTimeLog).WithName(nameof(UpdateTimeLog)).RequireAuthorization(AuthorizationPolicies.FreelancerOnly);
        group.MapDelete("/{id:guid}", DeleteTimeLog).WithName(nameof(DeleteTimeLog)).RequireAuthorization(AuthorizationPolicies.FreelancerOnly);
    }

    private static async Task<Results<Created<TimeLogDto>, UnauthorizedHttpResult>> CreateTimeLog(
        CreateTimeLogRequest request, HttpContext httpContext, IMessageBus bus, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return TypedResults.Unauthorized();

        var created = await bus.InvokeAsync<TimeLogDto>(request.ToCreateCommand(accountId), ct);
        return TypedResults.Created($"/timeLog/{created.Id}", created);
    }

    private static async Task<Ok<List<TimeLogDto>>> GetTimeLogs(IMessageBus bus, CancellationToken ct) =>
        TypedResults.Ok(await bus.InvokeAsync<List<TimeLogDto>>(new GetAllTimeLogsQuery(), ct));

    private static async Task<Ok<TimeLogDto>> GetTimeLogById(Guid id, IMessageBus bus, CancellationToken ct) =>
        TypedResults.Ok(await bus.InvokeAsync<TimeLogDto>(new GetTimeLogByIdQuery(id), ct));

    private static async Task<Results<NoContent, UnauthorizedHttpResult>> UpdateTimeLog(Guid id,
        UpdateTimeLogRequest request, HttpContext httpContext, IMessageBus bus, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return TypedResults.Unauthorized();

        await bus.InvokeAsync(request.ToUpdateCommand(id, accountId), ct);
        return TypedResults.NoContent();
    }

    private static async Task<Results<NoContent, UnauthorizedHttpResult>> DeleteTimeLog(Guid id,
        HttpContext httpContext, IMessageBus bus, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return TypedResults.Unauthorized();

        await bus.InvokeAsync(new DeleteTimeLogCommand(id, accountId), ct);
        return TypedResults.NoContent();
    }
}
