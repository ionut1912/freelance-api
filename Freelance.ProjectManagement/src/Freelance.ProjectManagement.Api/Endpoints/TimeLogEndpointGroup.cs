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

    private static async Task<Created<TimeLogDto>> CreateTimeLog(CreateTimeLogRequest request, IMessageBus bus,
        CancellationToken ct)
    {
        var created = await bus.InvokeAsync<TimeLogDto>(request.ToCreateCommand(), ct);
        return TypedResults.Created($"/timeLog/{created.Id}", created);
    }

    private static async Task<Ok<List<TimeLogDto>>> GetTimeLogs(IMessageBus bus, CancellationToken ct) =>
        TypedResults.Ok(await bus.InvokeAsync<List<TimeLogDto>>(new GetAllTimeLogsQuery(), ct));

    private static async Task<Ok<TimeLogDto>> GetTimeLogById(Guid id, IMessageBus bus, CancellationToken ct) =>
        TypedResults.Ok(await bus.InvokeAsync<TimeLogDto>(new GetTimeLogByIdQuery(id), ct));

    private static async Task<NoContent> UpdateTimeLog(Guid id, UpdateTimeLogRequest request, IMessageBus bus,
        CancellationToken ct)
    {
        await bus.InvokeAsync(request.ToUpdateCommand(id), ct);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> DeleteTimeLog(Guid id, IMessageBus bus, CancellationToken ct)
    {
        await bus.InvokeAsync(new DeleteTimeLogCommand(id), ct);
        return TypedResults.NoContent();
    }
}
