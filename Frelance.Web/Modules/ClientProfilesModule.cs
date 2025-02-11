using Frelance.Application.Mediatr.Commands.ClientProfiles;
using Frelance.Application.Mediatr.Queries.ClientProfiles;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.ClientProfile;
using Frelance.Contracts.Requests.Common;
using Frelance.Web.Extensions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;

namespace Frelance.Web.Modules
{
    public static class ClientProfilesModule
    {
        public static void AddClientProfilesEndpoints(this IEndpointRouteBuilder app)
        {
            var createClientProfileEndpoint = app.MapPost("/api/clientprofiles",
                    async (IMediator mediator, [FromForm] CreateClientProfileRequest uploadDto, CancellationToken ct) =>
                    {
                        var address = new AddressDto(
                            uploadDto.AddressCountry,
                            uploadDto.AddressCity,
                            uploadDto.AddressStreet,
                            uploadDto.AddressStreetNumber,
                            uploadDto.AddressZip);
                        var command = new AddClientProfileCommand(address, uploadDto.Bio, uploadDto.ProfileImage);
                        var result = await mediator.Send(command, ct);
                        return Results.Ok(result);
                    })
                .Accepts<ClientProfileDto>("multipart/form-data")
                .WithTags("ClientProfiles")
                .RequireAuthorization("ClientRole")
                .WithMetadata(new IgnoreAntiforgeryTokenAttribute());
            app.MapGet("/api/clientProfiles/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var clientProfile = await mediator.Send(new GetClientProfileByIdQuery(id), ct);
                return Results.Ok(clientProfile);
            }).WithTags("ClientProfiles").
            RequireAuthorization("ClientRole");
            app.MapGet("/api/clientProfiles", async (IMediator mediator, [FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken ct) =>
            {
                var paginatedClientProfiles = await mediator.Send(new GetClientProfilesQuery
                    (new PaginationParams { PageSize = pageSize, PageNumber = pageNumber }), ct);
                return Results.Extensions.OkPaginationResult(paginatedClientProfiles.PageSize, paginatedClientProfiles.CurrentPage,
                    paginatedClientProfiles.TotalCount, paginatedClientProfiles.TotalPages, paginatedClientProfiles.Items);
            }).WithTags("ClientProfiles").
            RequireAuthorization("ClientRole");
            createClientProfileEndpoint.RemoveAntiforgery();

        }

        private static void RemoveAntiforgery(this RouteHandlerBuilder builder)
        {
            builder.Add(builder =>
                     {
                         var antiforgeryMetadata = builder.Metadata
                             .Where(m => m is IAntiforgeryMetadata)
                             .ToList();

                         foreach (var item in antiforgeryMetadata)
                         {
                             builder.Metadata.Remove(item);
                         }
                     });

        }
    }
}