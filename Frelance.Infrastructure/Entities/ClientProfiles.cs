namespace Frelance.Infrastructure.Entities;

public class ClientProfiles : BaseUserProfile
{
    public List<Projects> Projects { get; set; } = [];
}