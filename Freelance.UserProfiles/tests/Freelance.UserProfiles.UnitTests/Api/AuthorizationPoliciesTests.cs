using Freelance.UserProfiles.Api;

namespace Freelance.UserProfiles.UnitTests.Api;

public sealed class AuthorizationPoliciesTests
{
    [Fact]
    public void PoliciesAndRoles_ArePairedInOrder()
    {
        AuthorizationPolicies.Policies.Should().Equal(AuthorizationPolicies.ClientOnly, AuthorizationPolicies.FreelancerOnly);
        AuthorizationPolicies.Roles.Should().Equal("Client", "Freelancer");
    }
}
