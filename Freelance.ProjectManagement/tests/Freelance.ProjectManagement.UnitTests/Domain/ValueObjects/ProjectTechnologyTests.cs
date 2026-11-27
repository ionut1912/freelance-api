using Freelance.ProjectManagement.Domain.ValueObjects;

namespace Freelance.ProjectManagement.UnitTests.Domain.ValueObjects;

public sealed class ProjectTechnologyTests
{
    [Fact]
    public void Create_SetsTechnology_AndEqualityIsByValue()
    {
        ProjectTechnology.Create("dotnet").Technology.Should().Be("dotnet");
        ProjectTechnology.Create("dotnet").Should().Be(new ProjectTechnology("dotnet"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_Blank_Throws(string value) =>
        FluentActions.Invoking(() => ProjectTechnology.Create(value)).Should().Throw<ArgumentException>();
}
