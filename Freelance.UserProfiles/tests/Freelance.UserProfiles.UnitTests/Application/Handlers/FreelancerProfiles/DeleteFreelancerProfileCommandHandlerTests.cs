using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.UnitTests.TestData;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.UserProfiles.UnitTests.Application.Handlers.FreelancerProfiles;

public sealed class DeleteFreelancerProfileCommandHandlerTests : HandlerTestBase
{
    private DeleteFreelancerProfileCommandHandler Handler =>
        new(Freelancers.Object, UnitOfWork.Object, NullLogger<DeleteFreelancerProfileCommandHandler>.Instance);

    [Fact]
    public async Task Handle_Missing_ThrowsProfileNotFound()
    {
        GivenFreelancerById(null);

        await Handler.Invoking(h => h.Handle(new DeleteFreelancerProfileCommand(Guid.NewGuid()), Ct)).Should().ThrowAsync<ProfileNotFoundException>();
    }

    [Fact]
    public async Task Handle_Existing_DeletesAndSaves()
    {
        var profile = Profiles.Freelancer();
        GivenFreelancerById(profile);

        await Handler.Handle(new DeleteFreelancerProfileCommand(profile.Id), Ct);

        Freelancers.Verify(r => r.Delete(profile), Times.Once);
        VerifySaved();
    }
}
