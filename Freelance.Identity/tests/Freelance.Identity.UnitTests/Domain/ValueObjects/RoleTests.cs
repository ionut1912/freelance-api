using Freelance.Identity.Domain.ValueObjects;

namespace Freelance.Identity.UnitTests.Domain.ValueObjects;

public sealed class RoleTests
{
    [Theory]
    [InlineData("Client")]
    [InlineData("Freelancer")]
    public void FromName_KnownRole_ReturnsRoleWithThatValue(string name) =>
        Role.FromName(name).Value.Should().Be(name);

    [Theory]
    [InlineData("Admin")]
    [InlineData("client")]
    [InlineData("")]
    [InlineData(null)]
    public void IsValid_UnknownRole_ReturnsFalse(string? name) => Role.IsValid(name).Should().BeFalse();

    [Fact]
    public void FromName_UnknownRole_Throws() =>
        FluentActions.Invoking(() => Role.FromName("Admin")).Should().Throw<ArgumentOutOfRangeException>();

    [Fact]
    public void TryFromName_KnownRole_ReturnsSingletonInstance()
    {
        Role.TryFromName("Client", out var role).Should().BeTrue();
        role.Should().BeSameAs(Role.Client);
    }

    [Fact]
    public void Equality_IsBasedOnValue()
    {
        Role.FromName("Freelancer").Should().Be(Role.Freelancer);
        Role.Client.Should().NotBe(Role.Freelancer);
    }
}
