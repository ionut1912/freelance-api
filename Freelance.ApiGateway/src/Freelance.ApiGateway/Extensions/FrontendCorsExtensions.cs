namespace Freelance.ApiGateway.Extensions;

public static class FrontendCorsExtensions
{
    public const string PolicyName = "frontend";

    extension(IServiceCollection services)
    {
        /// <summary>Allows the origins in Cors:AllowedOrigins plus FRONTEND_BASE_URL, with credentials.</summary>
        public IServiceCollection AddFrontendCors(IConfiguration configuration)
        {
            string[] allowedOrigins =
            [
                .. configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [],
                .. configuration["FRONTEND_BASE_URL"] is { Length: > 0 } frontendUrl ? [frontendUrl] : Array.Empty<string>()
            ];

            return services.AddCors(options => options.AddPolicy(PolicyName, policy => policy
                .WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()));
        }
    }
}
