using Freelance.ProjectManagement.Domain.ValueObjects;

namespace Freelance.ProjectManagement.UnitTests.Domain.ValueObjects;

public sealed class ProjectTaskStatusTests
{
    [Theory]
    [InlineData("New")]
    [InlineData("InProgress")]
    [InlineData("Review")]
    [InlineData("Done")]
    public void FromName_KnownValue_RoundTrips(string value) => ProjectTaskStatus.FromName(value).Value.Should().Be(value);

    [Theory]
    [InlineData("bogus")]
    [InlineData("")]
    [InlineData(null)]
    public void IsValid_UnknownValue_ReturnsFalse(string? value) => ProjectTaskStatus.IsValid(value).Should().BeFalse();

    [Fact]
    public void FromName_UnknownValue_Throws() =>
        FluentActions.Invoking(() => ProjectTaskStatus.FromName("bogus")).Should().Throw<ArgumentOutOfRangeException>();

    [Fact]
    public void All_ContainsEveryValue_AndEqualityIsByValue()
    {
        ProjectTaskStatus.All.Select(x => x.Value).Should().Equal("New", "InProgress", "Review", "Done");
        ProjectTaskStatus.FromName("New").Should().Be(ProjectTaskStatus.New);
    }
}
