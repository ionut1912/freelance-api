using System.Linq.Expressions;
using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Interfaces;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.UnitTests.Application.Handlers;

public abstract class HandlerTestBase
{
    protected Mock<IProjectRepository> ProjectRepository { get; } = new();
    protected Mock<IProjectTaskRepository> TaskRepository { get; } = new();
    protected Mock<ITimeLogRepository> TimeLogRepository { get; } = new();
    protected Mock<IUnitOfWork> UnitOfWork { get; } = new();
    protected CancellationToken Ct { get; } = TestContext.Current.CancellationToken;

    protected void GivenProject(Project? project, bool tracked = false)
    {
        if (tracked)
            ProjectRepository.Setup(r => r.GetByIdWithTrackingAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Project, object>>[]>())).ReturnsAsync(project);
        else
            ProjectRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Project, object>>[]>())).ReturnsAsync(project!);
    }

    protected void GivenTask(ProjectTask? task, bool tracked = false)
    {
        if (tracked)
            TaskRepository.Setup(r => r.GetByIdWithTrackingAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<ProjectTask, object>>[]>())).ReturnsAsync(task);
        else
            TaskRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<ProjectTask, object>>[]>())).ReturnsAsync(task!);
    }

    protected void GivenTimeLog(TimeLog? timeLog) =>
        TimeLogRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(),
            It.IsAny<Expression<Func<TimeLog, object>>[]>())).ReturnsAsync(timeLog!);

    protected void VerifySaved() => UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);

    protected void VerifyNotSaved() => UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
}
