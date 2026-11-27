using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.ValueObjects;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Microsoft.Extensions.DependencyInjection;

namespace Freelance.UserProfiles.IntegrationTests.Infrastructure;

/// <summary>The column limits the validators mirror; shared by both profile types.</summary>
public sealed class BaseProfileConfigurationTests(UserProfilesApiFactory factory)
{
    [Theory]
    [InlineData(typeof(ClientProfile))]
    [InlineData(typeof(FreelancerProfile))]
    public void Configure_LimitsBioAndAddressColumns(Type profile)
    {
        using var scope = factory.Services.CreateScope();
        var entity = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Model.FindEntityType(profile)!;
        var address = entity.FindNavigation(nameof(BaseUserProfile.Address))!.TargetEntityType;

        entity.FindProperty(nameof(BaseUserProfile.Bio))!.GetMaxLength().Should().Be(100);
        entity.FindProperty(nameof(BaseUserProfile.Image))!.GetMaxLength().Should().BeNull();
        address.FindProperty(nameof(Address.Street))!.GetMaxLength().Should().Be(200);
        address.FindProperty(nameof(Address.ZipCode))!.GetMaxLength().Should().Be(20);
    }
}
