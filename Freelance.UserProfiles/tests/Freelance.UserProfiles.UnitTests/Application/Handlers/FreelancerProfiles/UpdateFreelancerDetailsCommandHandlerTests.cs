using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.UnitTests.TestData;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.UserProfiles.UnitTests.Application.Handlers.FreelancerProfiles;

public sealed class UpdateFreelancerDetailsCommandHandlerTests : HandlerTestBase
{
    private UpdateFreelancerDetailsCommandHandler Handler =>
        new(Freelancers.Object, Skills.Object, UnitOfWork.Object, NullLogger<UpdateFreelancerDetailsCommandHandler>.Instance);

    private static UpdateFreelancerDetailsCommand Command(Guid id) =>
        new(id, ["English", "German"], ["Rust"], ["Systems"], "6y", 60, "USD", "https://y.io");

    [Fact]
    public async Task Handle_Missing_ThrowsProfileNotFound()
    {
        GivenFreelancerById(null);

        await Handler.Invoking(h => h.Handle(Command(Guid.NewGuid()), Ct)).Should().ThrowAsync<ProfileNotFoundException>();
        VerifyNotSaved();
    }

    [Fact]
    public async Task Handle_UpdatesTrackedProfileAndSaves()
    {
        var profile = Profiles.Freelancer();
        GivenFreelancerById(profile);

        await Handler.Handle(Command(profile.Id), Ct);

        profile.ForeignLanguages.Select(l => l.Language).Should().Equal("English", "German");
        profile.Skills.Should().ContainSingle(s => s.ProgrammingLanguage == "Rust");
        profile.Rate.Currency.Should().Be("USD");
        Freelancers.Verify(r => r.Update(profile), Times.Never, "the profile is tracked");
        VerifySaved();
    }
}
