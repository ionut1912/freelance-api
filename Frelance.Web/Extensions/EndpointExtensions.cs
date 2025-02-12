using Microsoft.AspNetCore.Antiforgery;

namespace Frelance.Web.Extensions;

public static class EndpointExtensions
{
    public static void RemoveAntiforgery(this RouteHandlerBuilder builder)
    {
        builder.Add(endpointBuilder =>
        {
            var antiforgeryMetadata = endpointBuilder.Metadata
                .Where(m => m is IAntiforgeryMetadata)
                .ToList();

            foreach (var item in antiforgeryMetadata)
            {
                endpointBuilder.Metadata.Remove(item);
            }
        });

    }

}