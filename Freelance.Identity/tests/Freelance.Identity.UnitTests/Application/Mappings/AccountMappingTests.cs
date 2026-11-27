using Freelance.Identity.Application.Mappings;
using Freelance.Identity.UnitTests.TestData;

namespace Freelance.Identity.UnitTests.Application.Mappings;

public sealed class AccountMappingTests
{
    [Fact]
    public void ToDto_MapsAllFields_AndNeverExposesThePassword()
    {
        var account = Accounts.Client();

        var dto = account.ToDto("token");

        dto.Should().BeEquivalentTo(new
        {
            account.Id,
            account.Email,
            account.PhoneNumber,
            account.Username,
            Token = "token",
            Role = "Client",
            account.IsBlocked,
            account.BlockedAt
        });
        dto.GetType().GetProperty("Password").Should().BeNull();
    }

    [Fact]
    public void ToDto_WithoutToken_HasNullToken() => Accounts.Client().ToDto().Token.Should().BeNull();
}
