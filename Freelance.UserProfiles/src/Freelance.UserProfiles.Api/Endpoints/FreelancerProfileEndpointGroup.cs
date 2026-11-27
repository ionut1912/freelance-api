using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Queies;
using Freelance.UserProfiles.Application.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Api.Endpoints;
using Shared.Api.Extensions;
using Wolverine;

namespace Freelance.UserProfiles.Api.Endpoints;

public sealed class FreelancerProfileEndpointGroup : EndpointGroup
{
    public override void Map(IEndpointRouteBuilder app)
    {
        // Explicit prefix: it is part of the public contract routed by the API gateway (/freelancer-profiles/*).
        var group = app.MapGroup("/freelancer-profiles").WithTags("Freelancer Profile").RequireAuthorization();
        var freelancers = group.MapGroup(string.Empty).RequireAuthorization(AuthorizationPolicies.FreelancerOnly);

        group.MapGet("", GetFreelancersProfiles).WithName(nameof(GetFreelancersProfiles));
        group.MapPut("/rating/{id:guid}", UpdateFreelancerRating).WithName(nameof(UpdateFreelancerRating))
            .RequireAuthorization(AuthorizationPolicies.ClientOnly);

        freelancers.MapPost("", CreateFreelancerProfile).WithName(nameof(CreateFreelancerProfile));
        freelancers.MapGet("/current", GetCurrentFreelancerProfile).WithName(nameof(GetCurrentFreelancerProfile));
        freelancers.MapPut("/details/{id:guid}", UpdateFreelancerDetails).WithName(nameof(UpdateFreelancerDetails));
        freelancers.MapPut("/address/{id:guid}", UpdateFreelancerAddress).WithName(nameof(UpdateFreelancerAddress));
        freelancers.MapPut("/data/{id:guid}", UpdateFreelancerData).WithName(nameof(UpdateFreelancerData));
        freelancers.MapPut("/verify", VerifyFreelancerProfile).WithName(nameof(VerifyFreelancerProfile));
        freelancers.MapDelete("/{id:guid}", DeleteFreelancerProfile).WithName(nameof(DeleteFreelancerProfile));
    }

    private static async Task<Results<Created<FreelancerProfileDto>, UnauthorizedHttpResult>> CreateFreelancerProfile(
        CreateFreelancerProfileRequest request, HttpContext httpContext, IMessageBus bus, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return TypedResults.Unauthorized();

        var created = await bus.InvokeAsync<FreelancerProfileDto>(request.ToCreateCommand(accountId), ct);
        return TypedResults.Created($"/freelancer-profiles/{created.Id}", created);
    }

    private static async Task<Ok<List<FreelancerProfileDto>>> GetFreelancersProfiles(IMessageBus bus,
        CancellationToken ct) =>
        TypedResults.Ok(await bus.InvokeAsync<List<FreelancerProfileDto>>(new GetFreelancerProfilesQuery(), ct));

    private static async Task<Results<Ok<FreelancerProfileDto>, UnauthorizedHttpResult>> GetCurrentFreelancerProfile(
        HttpContext httpContext, IMessageBus bus, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return TypedResults.Unauthorized();

        return TypedResults.Ok(
            await bus.InvokeAsync<FreelancerProfileDto>(new GetLoggedInFreelancerProfileQuery(accountId), ct));
    }

    private static async Task<Results<NoContent, UnauthorizedHttpResult>> UpdateFreelancerDetails(Guid id, UpdateFreelancerDetailRequest request,
        HttpContext httpContext, IMessageBus bus, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return TypedResults.Unauthorized();

        await bus.InvokeAsync(request.ToUpdateDetailsCommand(id, accountId), ct);
        return TypedResults.NoContent();
    }

    private static async Task<Results<NoContent, UnauthorizedHttpResult>> UpdateFreelancerAddress(Guid id, UpdateProfileAddressRequest request,
        HttpContext httpContext, IMessageBus bus, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return TypedResults.Unauthorized();

        await bus.InvokeAsync(request.ToUpdateFreelancerAddressCommand(id, accountId), ct);
        return TypedResults.NoContent();
    }

    private static async Task<Results<NoContent, UnauthorizedHttpResult>> UpdateFreelancerData(Guid id, UpdateProfileDataRequest request,
        HttpContext httpContext, IMessageBus bus, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return TypedResults.Unauthorized();

        await bus.InvokeAsync(request.ToUpdateFreelancerDataCommand(id, accountId), ct);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> UpdateFreelancerRating(Guid id, UpdateFreelancerProfileRatingRequest request,
        IMessageBus bus, CancellationToken ct)
    {
        await bus.InvokeAsync(request.ToUpdateRatingCommand(id), ct);
        return TypedResults.NoContent();
    }

    private static async Task<Results<NoContent, UnauthorizedHttpResult>> DeleteFreelancerProfile(Guid id,
        HttpContext httpContext, IMessageBus bus, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return TypedResults.Unauthorized();

        await bus.InvokeAsync(new DeleteFreelancerProfileCommand(id, accountId), ct);
        return TypedResults.NoContent();
    }

    /// <summary>Starts an asynchronous face verification; the result arrives later through RabbitMQ.</summary>
    private static async Task<Results<NoContent, UnauthorizedHttpResult>> VerifyFreelancerProfile(
        VerifyProfileRequest request, HttpContext httpContext, IMessageBus bus, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return TypedResults.Unauthorized();

        await bus.InvokeAsync(request.ToVerifyFreelancerCommand(accountId), ct);
        return TypedResults.NoContent();
    }
}
