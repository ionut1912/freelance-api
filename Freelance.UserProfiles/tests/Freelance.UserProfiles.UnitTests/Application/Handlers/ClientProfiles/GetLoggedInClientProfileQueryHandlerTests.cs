using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Queries;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.UnitTests.TestData;

namespace Freelance.UserProfiles.UnitTests.Application.Handlers.ClientProfiles;

public sealed class GetLoggedInClientProfileQueryHandlerTests : HandlerTestBase
{
    [Fact]
    public async Task Handle_Missing_ThrowsProfileNotFound()
    {
        GivenClientByAccount(null);

        await new GetLoggedInClientProfileQueryHandler(Clients.Object)
            .Invoking(h => h.Handle(new GetLoggedInClientProfileQuery(Guid.NewGuid()), Ct)).Should().ThrowAsync<ProfileNotFoundException>();
    }

    [Fact]
    public async Task Handle_Existing_ReturnsDto()
    {
        var profile = Profiles.Client();
        GivenClientByAccount(profile);

        (await new GetLoggedInClientProfileQueryHandler(Clients.Object).Handle(new GetLoggedInClientProfileQuery(profile.AccountId), Ct))
            .Id.Should().Be(profile.Id);
    }
}
