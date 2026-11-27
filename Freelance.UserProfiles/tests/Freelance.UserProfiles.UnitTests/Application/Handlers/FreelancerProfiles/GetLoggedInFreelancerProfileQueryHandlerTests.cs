using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Queies;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.UnitTests.TestData;

namespace Freelance.UserProfiles.UnitTests.Application.Handlers.FreelancerProfiles;

public sealed class GetLoggedInFreelancerProfileQueryHandlerTests : HandlerTestBase
{
    [Fact]
    public async Task Handle_Missing_ThrowsProfileNotFound()
    {
        GivenFreelancerByAccount(null);

        await new GetLoggedInFreelancerProfileQueryHandler(Freelancers.Object)
            .Invoking(h => h.Handle(new GetLoggedInFreelancerProfileQuery(Guid.NewGuid()), Ct)).Should().ThrowAsync<ProfileNotFoundException>();
    }

    [Fact]
    public async Task Handle_Existing_ReturnsDto()
    {
        var profile = Profiles.Freelancer();
        GivenFreelancerByAccount(profile);

        (await new GetLoggedInFreelancerProfileQueryHandler(Freelancers.Object).Handle(new GetLoggedInFreelancerProfileQuery(profile.AccountId), Ct))
            .Id.Should().Be(profile.Id);
    }
}
