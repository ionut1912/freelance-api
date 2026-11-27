using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.ValueObjects;

namespace Freelance.UserProfiles.UnitTests.TestData;

internal static class Profiles
{
    public static readonly AddressDto Address = new("Main", "Bucharest", "B", "010101", "RO", "10");

    public static ClientProfile Client(Guid? accountId = null) =>
        ClientProfile.Create(accountId ?? Guid.NewGuid(), "Main", "Bucharest", "B", "010101", "RO", "10", "Bio", "img");

    public static FreelancerProfile Freelancer(Guid? accountId = null)
    {
        var profile = FreelancerProfile.Create(accountId ?? Guid.NewGuid(), "Main", "Bucharest", "B", "010101", "RO", "10",
            "Bio", "img", "5y", 50, "eur", null, "https://x.io");
        profile.AddLanguages([FreelancerForeignLanguage.Create("English")]);
        profile.AddSkills([Skill.Create("C#", "Backend")]);
        return profile;
    }
}
