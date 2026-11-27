using Freelance.ApiGateway.Endpoints;
using Freelance.ApiGateway.Models;
using Freelance.ApiGateway.Services;

namespace Freelance.ApiGateway.Extensions;

public static class AggregatedOpenApiExtensions
{
    private static readonly TimeSpan DownstreamTimeout = TimeSpan.FromSeconds(10);
    private static readonly TimeSpan DocumentCacheDuration = TimeSpan.FromMinutes(1);

    extension(IServiceCollection services)
    {
        /// <summary>Registers the aggregator, its settings and the output cache of the aggregated document.</summary>
        public IServiceCollection AddAggregatedOpenApi(IConfiguration configuration)
        {
            services.Configure<OpenApiAggregationOptions>(configuration);
            services.AddOutputCache(options => options.AddPolicy(OpenApiEndpoints.CachePolicy,
                policy => policy.Expire(DocumentCacheDuration)));
            services.AddHttpClient<OpenApiAggregatorService>(client => client.Timeout = DownstreamTimeout);
            return services;
        }
    }
}
