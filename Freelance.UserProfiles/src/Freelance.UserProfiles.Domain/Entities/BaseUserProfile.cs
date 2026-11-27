using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.ValueObjects;
using Shared.Domain.Common;

namespace Freelance.UserProfiles.Domain.Entities;

public abstract class BaseUserProfile : Entity
{
    public const int BioMaxLength = 100;

    protected BaseUserProfile(Guid accountId, string street, string city, string state, string zipCode, string country,
        string streetNumber, string bio, string image)
    {
        AccountId = accountId;
        Address = Address.Create(street, city, state, zipCode, country, streetNumber);
        Bio = bio;
        Image = image;
        IsVerified = false;
    }

    protected BaseUserProfile() // for EF Core
    {
    }

    public Guid AccountId { get; private set; }
    public Address Address { get; private set; } = Address.Create(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
    public string Bio { get; private set; } = string.Empty;
    public string Image { get; private set; } = string.Empty;
    public bool IsVerified { get; private set; }

    public void Verify()
    {
        IsVerified = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAddress(string street, string city, string state, string zipCode, string country,
        string streetNumber)
    {
        Address = Address.Create(street, city, state, zipCode, country, streetNumber);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateUserData(string newImage, string newBio)
    {
        if (Image == newImage) throw new ImageAlreadyExistsException("The image is the same as the current one");
        if (Bio == newBio) throw new BioAlreadyExistsException("The bio is the same as the current one");

        Bio = newBio;
        Image = newImage;
        // The verification was made against the previous image.
        IsVerified = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
