using Freelance.Face.Domain.Interfaces;
using Freelance.Face.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Freelance.Face.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        /// <summary>Registers the dlib face service (singleton: the models are loaded once).</summary>
        public IServiceCollection AddFaceInfrastructure() => services.AddSingleton<IFaceService, FaceService>();
    }
}
