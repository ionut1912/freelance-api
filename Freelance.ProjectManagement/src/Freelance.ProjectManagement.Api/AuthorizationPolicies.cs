namespace Freelance.ProjectManagement.Api;

/// <summary>Role-based authorization policies (role names match the <c>role</c> claim issued by Identity).</summary>
public static class AuthorizationPolicies
{
    public const string ClientOnly = nameof(ClientOnly);
    public const string FreelancerOnly = nameof(FreelancerOnly);

    public static List<string> Policies => [ClientOnly, FreelancerOnly];
    public static List<string> Roles => ["Client", "Freelancer"];
}
