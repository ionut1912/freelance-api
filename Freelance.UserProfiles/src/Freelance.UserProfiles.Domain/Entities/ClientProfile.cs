namespace Freelance.UserProfiles.Domain.Entities;

public class ClientProfile : BaseUserProfile
{
    private ClientProfile()
    {
    }

    private ClientProfile(Guid accountId,string street, string city, string state, string zipCode, string country,
        string streetNumber, string bio, string image)
        : base(accountId,street, city, state, zipCode, country, streetNumber, bio, image)
    {
    }

    public static ClientProfile Create(Guid accountId, string street, string city, string state, string zipCode,
        string country,
        string streetNumber, string bio, string image)
    {
        return new ClientProfile(accountId, street, city, state, zipCode, country, streetNumber, bio, image);
    }
}