using Freelance.ProjectManagement.Domain.ValueObjects;

namespace Freelance.ProjectManagement.UnitTests.Domain.ValueObjects;

public sealed class ProjectTaskPriorityTests
{
    [Theory]
    [InlineData("Low")]
    [InlineData("Medium")]
    [InlineData("High")]
    public void FromName_KnownValue_RoundTrips(string value) => ProjectTaskPriority.FromName(value).Value.Should().Be(value);

    [Theory]
    [InlineData("bogus")]
    [InlineData("")]
    [InlineData(null)]
    public void IsValid_UnknownValue_ReturnsFalse(string? value) => ProjectTaskPriority.IsValid(value).Should().BeFalse();

    [Fact]
    public void FromName_UnknownValue_Throws() =>
        FluentActions.Invoking(() => ProjectTaskPriority.FromName("bogus")).Should().Throw<ArgumentOutOfRangeException>();

    [Fact]
    public void All_ContainsEveryValue_AndEqualityIsByValue()
    {
        ProjectTaskPriority.All.Select(x => x.Value).Should().Equal("Low", "Medium", "High");
        ProjectTaskPriority.FromName("Low").Should().Be(ProjectTaskPriority.Low);
    }
}
