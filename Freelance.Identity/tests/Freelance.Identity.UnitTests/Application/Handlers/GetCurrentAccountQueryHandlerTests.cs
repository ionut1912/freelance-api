using Freelance.Identity.Application.Mediatr.Accounts.Handlers;
using Freelance.Identity.Application.Mediatr.Accounts.Query;
using Freelance.Identity.Domain.Exceptions;

namespace Freelance.Identity.UnitTests.Application.Handlers;

public sealed class GetCurrentAccountQueryHandlerTests : HandlerTestBase
{
    private GetCurrentAccountQueryHandler Handler => new(Accounts.Object);

    [Fact]
    public async Task Handle_UnknownUser_ThrowsAccountNotFound() =>
        await Handler.Invoking(h => h.Handle(new GetCurrentAccountQuery("ghost"), Ct))
            .Should().ThrowAsync<AccountNotFoundException>();

    [Fact]
    public async Task Handle_KnownUser_ReturnsDtoWithoutToken()
    {
        var account = TestData.Accounts.Client();
        GivenAccountByUsername("bob", account);

        var dto = await Handler.Handle(new GetCurrentAccountQuery("bob"), Ct);

        dto.Id.Should().Be(account.Id);
        dto.Token.Should().BeNull();
    }
}
