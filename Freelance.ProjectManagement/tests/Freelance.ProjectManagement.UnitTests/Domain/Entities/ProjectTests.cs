using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.ValueObjects;
using Freelance.ProjectManagement.UnitTests.TestData;

namespace Freelance.ProjectManagement.UnitTests.Domain.Entities;

public sealed class ProjectTests
{
    [Theory]
    [InlineData("", "desc")]
    [InlineData("title", " ")]
    public void Create_WithoutTitleOrDescription_Throws(string title, string description) =>
        FluentActions.Invoking(() => Project.Create(title, description, Projects.Deadline, 10, "EUR"))
            .Should().Throw<ArgumentException>();

    [Fact]
    public void Create_NormalizesDeadlineToUtc_AndCurrency()
    {
        var project = Project.Create("Site", "Build", new DateTime(2030, 5, 1, 10, 0, 0, DateTimeKind.Unspecified), 1000, "eur");

        project.Deadline.Kind.Should().Be(DateTimeKind.Utc);
        project.Budget.Amount.Should().Be(1000);
        project.Budget.Currency.Should().Be("EUR");
    }

    [Fact]
    public void AddTechnologies_IgnoresDuplicates()
    {
        var project = Projects.Project();

        project.AddTechnologies([ProjectTechnology.Create("dotnet"), ProjectTechnology.Create("react")]);
        project.UpdateTechnologies([ProjectTechnology.Create("dotnet"), ProjectTechnology.Create("vue")]);

        project.Technologies.Select(t => t.Technology).Should().Equal("dotnet", "react", "vue");
    }

    [Fact]
    public void Update_KeepsTitleAndDescriptionWhenEmpty_AndReplacesBudget()
    {
        var project = Projects.Project();

        project.Update("", "", Projects.Deadline.AddDays(1), 2000, "usd");

        project.Title.Should().Be("Site");
        project.Description.Should().Be("Build site");
        project.Deadline.Should().Be(Projects.Deadline.AddDays(1));
        project.Budget.Amount.Should().Be(2000);
        project.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Update_ReplacesTitleAndDescription()
    {
        var project = Projects.Project();

        project.Update("New", "New desc", Projects.Deadline, 1, "EUR");

        project.Title.Should().Be("New");
        project.Description.Should().Be("New desc");
    }

    [Fact]
    public void UpdateTask_UnknownTask_Throws() =>
        Projects.Project().Invoking(p => p.UpdateTask(Guid.NewGuid(), "t", "d", ProjectTaskStatus.Done, ProjectTaskPriority.Low))
            .Should().Throw<ProjectTaskNotFoundException>();

    [Fact]
    public void UpdateTask_KnownTask_UpdatesIt()
    {
        var project = Projects.Project();
        var task = Projects.Task(project.Id);
        project.AddTask(task);

        project.UpdateTask(task.Id, "API v2", "", ProjectTaskStatus.Done, ProjectTaskPriority.Low);

        project.Tasks.Should().ContainSingle().Which.Should().BeEquivalentTo(new
        {
            Title = "API v2",
            Description = "Build API",
            Status = ProjectTaskStatus.Done,
            Priority = ProjectTaskPriority.Low
        });
    }

    [Fact]
    public void AddTask_Null_Throws() => Projects.Project().Invoking(p => p.AddTask(null!)).Should().Throw<ArgumentNullException>();

    [Fact]
    public void AssignClientAndFreelancer_SetsIds()
    {
        var project = Projects.Project();
        var client = Guid.NewGuid();
        var freelancer = Guid.NewGuid();

        project.AssignClient(client);
        project.AssignFreelancer(freelancer);

        project.ClientId.Should().Be(client);
        project.FreelancerId.Should().Be(freelancer);
    }
}
