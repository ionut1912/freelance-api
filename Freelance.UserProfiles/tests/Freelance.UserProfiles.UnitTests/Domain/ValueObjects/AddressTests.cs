using Freelance.UserProfiles.Domain.ValueObjects;

namespace Freelance.UserProfiles.UnitTests.Domain.ValueObjects;

public sealed class AddressTests
{
    [Fact]
    public void Create_SetsComponents_AndEqualityIsByValue()
    {
        var address = Address.Create("Main", "Bucharest", "B", "010101", "RO", "10");

        address.Should().BeEquivalentTo(new { Street = "Main", City = "Bucharest", State = "B", ZipCode = "010101", Country = "RO", StreetNumber = "10" });
        address.Should().Be(Address.Create("Main", "Bucharest", "B", "010101", "RO", "10"));
        address.Should().NotBe(Address.Create("Main", "Cluj", "B", "010101", "RO", "10"));
    }

    [Fact]
    public void ToString_ListsAllComponents() =>
        Address.Create("Main", "Bucharest", "B", "010101", "RO", "10").ToString().Should().Be("Main, Bucharest, B 010101, RO, 10");

    [Fact]
    public void Create_NullComponent_Throws() =>
        FluentActions.Invoking(() => Address.Create("Main", null!, "B", "1", "RO", "1")).Should().Throw<ArgumentNullException>();
}
