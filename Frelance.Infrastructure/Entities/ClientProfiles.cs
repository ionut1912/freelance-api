namespace Frelance.Infrastructure.Entities;

public class ClientProfiles : BaseUserProfile
{
    public ClientProfiles() : base(null)
    {
    }

    public ClientProfiles(Users user) : base(user)
    {
    }
}