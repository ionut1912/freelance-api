using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.UnitTests.TestData;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.UserProfiles.UnitTests.Application.Handlers.FreelancerProfiles;

public sealed class UpdateFreelancerProfileDataCommandHandlerTests : HandlerTestBase
{
    private UpdateFreelancerProfileDataCommandHandler Handler =>
        new(Freelancers.Object, UnitOfWork.Object, NullLogger<UpdateFreelancerProfileDataCommandHandler>.Instance);

    [Fact]
    public async Task Handle_Missing_ThrowsProfileNotFound()
    {
        GivenFreelancerById(null);

        await Handler.Invoking(h => h.Handle(new UpdateFreelancerProfileDataCommand(Guid.NewGuid(), "b", "i"), Ct))
            .Should().ThrowAsync<ProfileNotFoundException>();
    }

    [Fact]
    public async Task Handle_SameBio_ThrowsWithoutSaving()
    {
        GivenFreelancerById(Profiles.Freelancer());

        await Handler.Invoking(h => h.Handle(new UpdateFreelancerProfileDataCommand(Guid.NewGuid(), "Bio", "new"), Ct))
            .Should().ThrowAsync<BioAlreadyExistsException>();
        VerifyNotSaved();
    }

    [Fact]
    public async Task Handle_UpdatesAndSaves()
    {
        var profile = Profiles.Freelancer();
        GivenFreelancerById(profile);

        await Handler.Handle(new UpdateFreelancerProfileDataCommand(profile.Id, "New", "new-img"), Ct);

        profile.Image.Should().Be("new-img");
        VerifySaved();
    }
}
