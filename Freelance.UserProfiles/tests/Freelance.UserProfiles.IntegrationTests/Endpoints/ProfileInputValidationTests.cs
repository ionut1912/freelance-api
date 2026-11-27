using System.Net;
using System.Net.Http.Json;
using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Microsoft.Extensions.DependencyInjection;

namespace Freelance.UserProfiles.IntegrationTests.Endpoints;

/// <summary>Input the database cannot store must be rejected as 400, never surface as 500.</summary>
public sealed class ProfileInputValidationTests(UserProfilesApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    private static string Long(int length) => new('a', length);

    private static Dictionary<string, object?> FreelancerBody() => new()
    {
        ["address"] = UserProfilesApiFactory.Address,
        ["bio"] = "Dev",
        ["image"] = "img",
        ["experience"] = "5y",
        ["amount"] = 50,
        ["currency"] = "EUR",
        ["portfolioUrl"] = "https://x.io",
        ["foreignLanguages"] = new[] { "English" },
        ["programmingLanguages"] = new[] { "C#" },
        ["areas"] = new[] { "Backend" }
    };

    public static TheoryData<string, object> InvalidFreelancerFields => new()
    {
        { "bio", Long(101) },
        { "currency", "EURO" },
        { "foreignLanguages", new[] { Long(101) } },
        { "foreignLanguages", new string?[] { null } },
        { "address", new AddressDto(Long(201), "c", "s", "z", "co", "1") },
        { "address", new AddressDto("s", "c", "s", Long(21), "co", "1") },
        { "address", new { street = "only street" } }
    };

    [Theory]
    [MemberData(nameof(InvalidFreelancerFields))]
    public async Task CreateFreelancerProfile_WithUnstorableValue_ReturnsBadRequest(string field, object value)
    {
        var body = FreelancerBody();
        body[field] = value;

        var response = await factory.CreateClient("Freelancer").PostAsJsonAsync("/freelancer-profiles", body, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateClientProfile_WithBioLongerThanColumn_ReturnsBadRequest()
    {
        var response = await factory.CreateClient("Client").PostAsJsonAsync("/client-profiles",
            new { address = UserProfilesApiFactory.Address, bio = Long(101), image = "img" }, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateFreelancerProfile_WithRepeatedSkillPair_StoresTheSkillOnce()
    {
        var body = FreelancerBody();
        var language = $"Zig-{Guid.NewGuid():N}";
        body["programmingLanguages"] = new[] { language, language };
        body["areas"] = new[] { "Systems", "Systems" };

        var response = await factory.CreateClient("Freelancer").PostAsJsonAsync("/freelancer-profiles", body, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        (await response.Content.ReadFromJsonAsync<FreelancerProfileDto>(_ct))!.ProgrammingLanguages
            .Should().Equal(language);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(6)]
    [InlineData(int.MaxValue)]
    public async Task UpdateRating_OutsideOneToFive_ReturnsBadRequest(int rating)
    {
        var profile = await UserProfilesApiFactory.CreateFreelancerProfileAsync(factory.CreateClient("Freelancer"), _ct);

        var response = await factory.CreateClient("Client")
            .PutAsJsonAsync($"/freelancer-profiles/rating/{profile.Id}", new { rating }, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateImage_OfVerifiedProfile_RemovesVerification()
    {
        var client = factory.CreateClient("Client");
        var profile = await UserProfilesApiFactory.CreateClientProfileAsync(client, _ct);
        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            (await context.ClientProfiles.FindAsync([profile.Id], _ct))!.Verify();
            await context.SaveChangesAsync(_ct);
        }

        (await client.PutAsJsonAsync($"/client-profiles/{profile.Id}/data", new { bio = "Other", image = "other" }, _ct))
            .EnsureSuccessStatusCode();

        (await client.GetFromJsonAsync<ClientProfileDto>("/client-profiles/current", _ct))!.IsVerified.Should().BeFalse();
    }
}
