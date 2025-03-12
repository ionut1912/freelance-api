using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace Frelance.Infrastructure.Entities;

[UsedImplicitly]
public class Users(FreelancerProfiles? freelancerProfiles, ClientProfiles? clientProfiles)
    : IdentityUser<int>
{
    public Users() : this(null, null)
    {
    }

    public FreelancerProfiles? FreelancerProfiles { get; } = freelancerProfiles;
    public ClientProfiles? ClientProfiles { get; } = clientProfiles;
    public List<Reviews> Reviews { get; } = [];
    public List<Proposals> Proposals { get; } = [];
    public DateTime CreatedAt { get; set; }
}