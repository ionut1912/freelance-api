using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Application.Validators.ProjectTask;

namespace Freelance.ProjectManagement.UnitTests.Application.Validators;

public sealed class UpdateProjectTaskCommandValidatorTests
{
    private static readonly UpdateProjectTaskCommand Valid = new(Guid.NewGuid(), Guid.NewGuid(), "", "", "Done", "Low", Guid.NewGuid());
    private readonly UpdateProjectTaskCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_Passes() => _validator.Validate(Valid).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_InvalidValues_ReportsEachField() =>
        _validator.Validate(Valid with { ProjectId = Guid.Empty, Status = "x", Priority = "y" }).Errors.Select(e => e.PropertyName)
            .Should().BeEquivalentTo("ProjectId", "Status", "Priority");

    [Theory]
    [InlineData(100, 500, true)]
    [InlineData(101, 500, false)]
    [InlineData(100, 501, false)]
    public void Validate_TextLengthBoundaries_MatchTheColumns(int title, int description, bool valid) =>
        _validator.Validate(new UpdateProjectTaskCommand(Guid.NewGuid(), Guid.NewGuid(), new string('a', title), new string('a', description), "New", "Low", Guid.NewGuid()))
            .IsValid.Should().Be(valid);
}
