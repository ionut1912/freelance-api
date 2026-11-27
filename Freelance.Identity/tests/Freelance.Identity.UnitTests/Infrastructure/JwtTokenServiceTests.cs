using System.Security.Claims;
using System.Text;
using Freelance.Identity.Infrastructure.Persistance.Repositories;
using Freelance.Identity.UnitTests.TestData;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Shared.Infra.Settings;

namespace Freelance.Identity.UnitTests.Infrastructure;

public sealed class JwtTokenServiceTests
{
    private static readonly JwtSettings Settings = new()
    {
        Key = "unit-test-signing-key-that-is-long-enough-123",
        Issuer = "issuer",
        Audience = "audience"
    };

    private static readonly DateTimeOffset Now = new(2030, 1, 1, 12, 0, 0, TimeSpan.Zero);

    private static JwtTokenService CreateService()
    {
        var time = new Mock<TimeProvider>();
        time.Setup(t => t.GetUtcNow()).Returns(Now);
        return new JwtTokenService(Options.Create(Settings), time.Object);
    }

    private static Task<TokenValidationResult> ValidateAsync(string token, string key) =>
        new JsonWebTokenHandler().ValidateTokenAsync(token, new TokenValidationParameters
        {
            ValidIssuer = Settings.Issuer,
            ValidAudience = Settings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateLifetime = false
        });

    [Fact]
    public async Task GenerateToken_ProducesSignedTokenWithAccountClaims()
    {
        var account = Accounts.Freelancer();

        var result = await ValidateAsync(CreateService().GenerateToken(account), Settings.Key);

        result.IsValid.Should().BeTrue();
        result.Claims.Should().Contain(new KeyValuePair<string, object>(JwtRegisteredClaimNames.Sub, account.Id.ToString()))
            .And.Contain(new KeyValuePair<string, object>(ClaimTypes.Name, "alice"))
            .And.Contain(new KeyValuePair<string, object>(ClaimTypes.Role, "Freelancer"))
            .And.ContainKey(JwtRegisteredClaimNames.Jti);
    }

    [Fact]
    public async Task GenerateToken_ExpiresTwoHoursAfterIssue()
    {
        var token = (JsonWebToken)(await ValidateAsync(CreateService().GenerateToken(Accounts.Client()), Settings.Key)).SecurityToken;

        token.IssuedAt.Should().Be(Now.UtcDateTime);
        token.ValidTo.Should().Be(Now.UtcDateTime.AddHours(2));
    }

    [Fact]
    public async Task GenerateToken_IsRejectedWhenValidatedWithAnotherKey() =>
        (await ValidateAsync(CreateService().GenerateToken(Accounts.Client()), "another-key-another-key-another-key-123"))
        .IsValid.Should().BeFalse();

    [Fact]
    public void GenerateToken_NullAccount_Throws() =>
        CreateService().Invoking(s => s.GenerateToken(null!)).Should().Throw<ArgumentNullException>();
}
