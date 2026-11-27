using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.UnitTests.TestData;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.UserProfiles.UnitTests.Application.Handlers.FreelancerProfiles;

public sealed class UpdateFreelancerProfileAddressCommandHandlerTests : HandlerTestBase
{
    private UpdateFreelancerProfileAddressCommandHandler Handler =>
        new(Freelancers.Object, UnitOfWork.Object, NullLogger<UpdateFreelancerProfileAddressCommandHandler>.Instance);

    [Fact]
    public async Task Handle_Missing_ThrowsProfileNotFound()
    {
        GivenFreelancerById(null);

        await Handler.Invoking(h => h.Handle(new UpdateFreelancerProfileAddressCommand(Guid.NewGuid(), Profiles.Address), Ct))
            .Should().ThrowAsync<ProfileNotFoundException>();
    }

    [Fact]
    public async Task Handle_UpdatesAddressAndSaves()
    {
        var profile = Profiles.Freelancer();
        GivenFreelancerById(profile);

        await Handler.Handle(new UpdateFreelancerProfileAddressCommand(profile.Id, new AddressDto("S", "Iasi", "IS", "7", "RO", "3")), Ct);

        profile.Address.City.Should().Be("Iasi");
        VerifySaved();
    }
}
