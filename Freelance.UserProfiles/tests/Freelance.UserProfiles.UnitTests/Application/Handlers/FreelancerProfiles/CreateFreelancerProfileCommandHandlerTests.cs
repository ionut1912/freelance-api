using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.UnitTests.TestData;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.UserProfiles.UnitTests.Application.Handlers.FreelancerProfiles;

public sealed class CreateFreelancerProfileCommandHandlerTests : HandlerTestBase
{
    private CreateFreelancerProfileCommandHandler Handler =>
        new(Freelancers.Object, Skills.Object, UnitOfWork.Object, NullLogger<CreateFreelancerProfileCommandHandler>.Instance);

    private static CreateFreelancerProfileCommand Command(Guid accountId) =>
        new(accountId, Profiles.Address, "Dev", "img", "5y", 50, "EUR", "https://x.io", ["English", "French"], ["C#", "Go"], ["Backend", "Backend"]);

    [Fact]
    public async Task Handle_ProfileAlreadyExists_Throws()
    {
        GivenFreelancerByAccount(Profiles.Freelancer());

        await Handler.Invoking(h => h.Handle(Command(Guid.NewGuid()), Ct)).Should().ThrowAsync<ProfileAllreadyExistsException>();
        VerifyNotSaved();
    }

    [Fact]
    public async Task Handle_NewProfile_WithSeveralLanguagesAndSkills_IsPersisted()
    {
        var accountId = Guid.NewGuid();

        var dto = await Handler.Handle(Command(accountId), Ct);

        Freelancers.Verify(r => r.AddAsync(It.Is<FreelancerProfile>(p => p.ForeignLanguages.Count == 2 && p.Skills.Count == 2), Ct), Times.Once);
        Skills.Verify(r => r.AddAsync(It.IsAny<Skill>(), Ct), Times.Exactly(2));
        VerifySaved();
        dto.Should().BeEquivalentTo(new { AccountId = accountId, Rating = 0, ForeignLanguages = new[] { "English", "French" } });
    }
}
