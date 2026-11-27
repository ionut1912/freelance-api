using System.Linq.Expressions;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Queies;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.UnitTests.TestData;

namespace Freelance.UserProfiles.UnitTests.Application.Handlers.FreelancerProfiles;

public sealed class GetFreelancerProfilesQueryHandlerTests : HandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsAllProfilesWithSkillsAndLanguages()
    {
        Freelancers.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FreelancerProfile, object>>[]>()))
            .ReturnsAsync([Profiles.Freelancer()]);

        var dtos = await new GetFreelancerProfilesQueryHandler(Freelancers.Object).Handle(new GetFreelancerProfilesQuery(), Ct);

        dtos.Should().ContainSingle().Which.ProgrammingLanguages.Should().Equal("C#");
        Freelancers.Verify(r => r.GetAllAsync(Ct, It.Is<Expression<Func<FreelancerProfile, object>>[]>(i => i.Length == 2)), Times.Once);
    }
}
