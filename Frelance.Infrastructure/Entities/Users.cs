using Microsoft.AspNetCore.Identity;

namespace Frelance.Infrastructure.Entities;

public class Users : IdentityUser<int>
{
    public  FreelancerProfiles FreelancerProfiles { get; set; }
    public  ClientProfiles ClientProfiles { get; set; }
    public List<Reviews> Reviews { get; set; } = [];
    public List<Proposals> Proposals { get; set; } = [];
}