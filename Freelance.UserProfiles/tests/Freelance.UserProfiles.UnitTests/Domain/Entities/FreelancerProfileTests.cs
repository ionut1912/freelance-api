using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.ValueObjects;
using Freelance.UserProfiles.UnitTests.TestData;

namespace Freelance.UserProfiles.UnitTests.Domain.Entities;

public sealed class FreelancerProfileTests
{
    [Fact]
    public void Create_SetsRateAndDefaultsRatingToZero()
    {
        var profile = Profiles.Freelancer();

        profile.Rate.Amount.Should().Be(50);
        profile.Rate.Currency.Should().Be("EUR");
        profile.Rating.Should().Be(0);
        profile.Experience.Should().Be("5y");
    }

    [Fact]
    public void AddLanguages_IgnoresDuplicates()
    {
        var profile = Profiles.Freelancer();

        profile.AddLanguages([FreelancerForeignLanguage.Create("English"), FreelancerForeignLanguage.Create("French")]);

        profile.ForeignLanguages.Select(l => l.Language).Should().Equal("English", "French");
    }

    [Fact]
    public void UpdateLanguages_KeepsUnchangedInstances_RemovesAndAdds()
    {
        var profile = Profiles.Freelancer();
        var english = profile.ForeignLanguages.Single();

        profile.UpdateLanguages([FreelancerForeignLanguage.Create("English"), FreelancerForeignLanguage.Create("German"),
            FreelancerForeignLanguage.Create("German")]);

        profile.ForeignLanguages.Select(l => l.Language).Should().Equal("English", "German");
        profile.ForeignLanguages.First().Should().BeSameAs(english, "unchanged rows must not be deleted and re-inserted");
    }

    [Fact]
    public void UpdateSkills_ReplacesSkills()
    {
        var profile = Profiles.Freelancer();

        profile.UpdateSkills([Skill.Create("Go", "Backend"), Skill.Create("Rust", "Systems")]);

        profile.Skills.Select(s => s.ProgrammingLanguage).Should().Equal("Go", "Rust");
    }

    [Fact]
    public void UpdateRatingAndDetails_ChangeValues()
    {
        var profile = Profiles.Freelancer();

        profile.UpdateRating(4);
        profile.UpdateFreelancerDetails("10y", 90, "usd", "https://y.io");

        profile.Should().BeEquivalentTo(new { Rating = 4, Experience = "10y", PortfolioUrl = "https://y.io" });
        profile.Rate.Amount.Should().Be(90);
    }

    [Fact]
    public void CollectionMutators_RejectNull()
    {
        var profile = Profiles.Freelancer();

        profile.Invoking(p => p.AddLanguages(null!)).Should().Throw<ArgumentNullException>();
        profile.Invoking(p => p.UpdateLanguages(null!)).Should().Throw<ArgumentNullException>();
        profile.Invoking(p => p.AddSkills(null!)).Should().Throw<ArgumentNullException>();
        profile.Invoking(p => p.UpdateSkills(null!)).Should().Throw<ArgumentNullException>();
    }
}
