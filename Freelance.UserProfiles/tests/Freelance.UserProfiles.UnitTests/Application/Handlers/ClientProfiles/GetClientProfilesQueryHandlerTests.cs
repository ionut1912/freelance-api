using System.Linq.Expressions;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Queries;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.UnitTests.TestData;

namespace Freelance.UserProfiles.UnitTests.Application.Handlers.ClientProfiles;

public sealed class GetClientProfilesQueryHandlerTests : HandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsAllProfiles()
    {
        Clients.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<ClientProfile, object>>[]>()))
            .ReturnsAsync([Profiles.Client(), Profiles.Client()]);

        (await new GetClientProfilesQueryHandler(Clients.Object).Handle(new GetClientProfilesQuery(), Ct)).Should().HaveCount(2);
    }
}
