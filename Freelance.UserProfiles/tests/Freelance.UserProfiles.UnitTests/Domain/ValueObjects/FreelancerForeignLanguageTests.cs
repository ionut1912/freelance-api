using Freelance.UserProfiles.Domain.ValueObjects;

namespace Freelance.UserProfiles.UnitTests.Domain.ValueObjects;

public sealed class FreelancerForeignLanguageTests
{
    [Fact]
    public void Create_SetsLanguage_AndEqualityIsByValue()
    {
        FreelancerForeignLanguage.Create("English").Language.Should().Be("English");
        FreelancerForeignLanguage.Create("English").Should().Be(FreelancerForeignLanguage.Create("English"));
        FreelancerForeignLanguage.Create("English").Should().NotBe(FreelancerForeignLanguage.Create("French"));
    }
}
