using Freelance.Face.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Wolverine;

[assembly: AssemblyFixture(typeof(Freelance.Face.IntegrationTests.FaceApiFactory))]

namespace Freelance.Face.IntegrationTests;

/// <summary>
/// Boots the real Face API. RabbitMQ is replaced by Wolverine's stubbed transports and the dlib service by a mock,
/// so the HTTP surface and the message flow are tested without native models or a broker.
/// </summary>
public sealed class FaceApiFactory : WebApplicationFactory<Program>
{
    public const string FrontendUrl = "https://frontend.test/";

    public Mock<IFaceService> FaceService { get; } = new();

    public IHost Host => Services.GetRequiredService<IHost>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.UseSetting("FRONTEND_BASE_URL", FrontendUrl);
        builder.UseSetting("OTEL_EXPORTER_OTLP_ENDPOINT", "http://localhost:4317");
        builder.UseSetting("OTEL_EXPORTER_OTLP_LOGS_ENDPOINT", "http://localhost:3100");
        builder.ConfigureTestServices(services =>
        {
            services.DisableAllExternalWolverineTransports();
            services.RemoveAll<IFaceService>();
            services.AddSingleton(FaceService.Object);
        });
    }
}
