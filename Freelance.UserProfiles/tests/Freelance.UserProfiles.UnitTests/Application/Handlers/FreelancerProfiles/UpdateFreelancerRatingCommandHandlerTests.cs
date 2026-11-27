using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.UnitTests.TestData;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.UserProfiles.UnitTests.Application.Handlers.FreelancerProfiles;

public sealed class UpdateFreelancerRatingCommandHandlerTests : HandlerTestBase
{
    private UpdateFreelancerRatingCommandHandler Handler =>
        new(Freelancers.Object, UnitOfWork.Object, NullLogger<UpdateFreelancerRatingCommandHandler>.Instance);

    [Fact]
    public async Task Handle_Missing_ThrowsProfileNotFound()
    {
        GivenFreelancerById(null);

        await Handler.Invoking(h => h.Handle(new UpdateFreelancerRatingCommand(Guid.NewGuid(), 5), Ct))
            .Should().ThrowAsync<ProfileNotFoundException>();
    }

    [Fact]
    public async Task Handle_UpdatesRatingAndSaves()
    {
        var profile = Profiles.Freelancer();
        GivenFreelancerById(profile);

        await Handler.Handle(new UpdateFreelancerRatingCommand(profile.Id, 5), Ct);

        profile.Rating.Should().Be(5);
        VerifySaved();
    }
}
