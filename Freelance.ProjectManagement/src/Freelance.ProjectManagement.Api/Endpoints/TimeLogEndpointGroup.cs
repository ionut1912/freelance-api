using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Querires;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Queries;
using Freelance.ProjectManagement.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Shared.Api.Infrastructure;
using Shared.Application.Mediator;

namespace Freelance.ProjectManagement.Api.Endpoints;

public class TimeLogEndpointGroup:EndpointGroup
{
    public override void Map(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(this);

        group.MapPost(CreeateTimeLog);
        group.MapGet(GetTimeLogs);
        group.MapGet(GetTimeLogById, "/{id:guid}");
        group.MapPut(UpdateTimelog, "/{id:guid}");
        group.MapDelete(DeleteTimeLog, "/{id:guid}");
    }

    [Authorize(Policy = "FreelancerOnly")]
    public static async Task<IResult> CreeateTimeLog(IMediator mediator, CreateTimeLogRequest createTimeLogRequest, CancellationToken cancellationToken)
    {
        var command = createTimeLogRequest.ToCreateCommand();
        var created = await mediator.Send(command, cancellationToken);
        return Results.Created($"/timeLog/{created.Id}", created);
    }

    [Authorize]
    public static async Task<IResult> GetTimeLogs(IMediator mediator, CancellationToken cancellationToken)
    {
        var query = new GetAllTimeLogsQuery();
        var timeLogs = await mediator.Send(query, cancellationToken);
        return Results.Ok(timeLogs);
    }

    [Authorize]
    public static async Task<IResult> GetTimeLogById(IMediator mediator, Guid id, CancellationToken cancellationToken)
    {
        var query = new GetTimeLogByIdQuery(id);
        var timeLog = await mediator.Send(query, cancellationToken);
        return Results.Ok(timeLog);
    }

    [Authorize(Policy = "FreelancerOnly")]
    public static async Task<IResult> UpdateTimelog(IMediator mediator, Guid id, UpdateTimeLogRequest updateTimeLogRequest, CancellationToken cancellationToken)
    {
        var comand = updateTimeLogRequest.ToUpdateCommand(id);
        await mediator.Send(comand, cancellationToken);
        return Results.NoContent();
    }

    [Authorize(Policy = "FreelancerOnly")]
    public static async Task<IResult> DeleteTimeLog(IMediator mediator, Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteTimeLogCommand(id);
        await mediator.Send(command, cancellationToken);
        return Results.NoContent();
    }
}
