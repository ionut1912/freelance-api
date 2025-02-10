using Frelance.Application.Mediatr.Commands.ClientProfiles;
using Frelance.Contracts.Dtos;
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
            var endpoint = app.MapPost("/api/clientprofiles",
                    async (IMediator mediator, [FromForm] ClientProfileDto uploadDto, CancellationToken ct) =>
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

            // Remove antiforgery metadata from the endpoint.
            endpoint.Add(builder =>
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