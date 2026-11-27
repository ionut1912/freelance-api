using System.Linq.Expressions;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Interfaces;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.UnitTests.Application.Handlers;

public abstract class HandlerTestBase
{
    protected Mock<IClientProfileRepository> Clients { get; } = new();
    protected Mock<IFreelancerProfileRepository> Freelancers { get; } = new();
    protected Mock<ISkillRepository> Skills { get; } = new();
    protected Mock<IUnitOfWork> UnitOfWork { get; } = new();
    protected CancellationToken Ct { get; } = TestContext.Current.CancellationToken;

    protected void GivenClientById(ClientProfile? profile) =>
        Clients.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(),
            It.IsAny<Expression<Func<ClientProfile, object>>[]>())).ReturnsAsync(profile!);

    protected void GivenClientByAccount(ClientProfile? profile) =>
        Clients.Setup(r => r.GetLoggedInClientProfileAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(profile);

    protected void GivenFreelancerById(FreelancerProfile? profile)
    {
        Freelancers.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(),
            It.IsAny<Expression<Func<FreelancerProfile, object>>[]>())).ReturnsAsync(profile!);
        Freelancers.Setup(r => r.GetByIdWithTrackingAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(),
            It.IsAny<Expression<Func<FreelancerProfile, object>>[]>())).ReturnsAsync(profile);
    }

    protected void GivenFreelancerByAccount(FreelancerProfile? profile) =>
        Freelancers.Setup(r => r.GetLoggedInFreelancerProfileAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(profile);

    protected void VerifySaved() => UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);

    protected void VerifyNotSaved() => UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
}
