using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Domain.ValueObjects;

namespace Freelance.UserProfiles.UnitTests.Application.Mappings;

public sealed class AddressMappingsTests
{
    [Fact]
    public void ToDto_MapsEveryComponentToTheMatchingField() =>
        Address.Create("Main", "Bucharest", "B", "010101", "RO", "10").ToDto()
            .Should().BeEquivalentTo(new { Street = "Main", City = "Bucharest", State = "B", ZipCode = "010101", Country = "RO", StreetNumber = "10" });
}
