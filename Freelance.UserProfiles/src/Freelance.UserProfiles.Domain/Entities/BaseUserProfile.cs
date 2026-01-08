using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.ValueObjects;
using Shared.Domain.Common;

namespace Freelance.UserProfiles.Domain.Entities;

public abstract class BaseUserProfile : Entity
{
    protected BaseUserProfile(Guid accountId, string street, string city, string state, string zipCode, string country,
        string streetNumber, string bio, string image)
    {
        AccountId = accountId;
        Address = Address.Create(street, city, state, zipCode, country, streetNumber);
        Bio = bio;
        Image = image;
        CreatedAt = DateTime.UtcNow;
        IsVerified = false;
    }

    protected BaseUserProfile()
    {
    }

    public Guid AccountId { get; private set; }
    public Address Address { get; private set; } = Address.Create(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
    public string Bio { get; private set; } = string.Empty;
    public string Image { get; private set; } = string.Empty;
    public bool IsVerified { get; private set; } = false;

    public void Verify()
    {
        IsVerified = true;
    }

    public void UpdateAddress(string street, string city, string state, string zipCode, string country,
        string streetNumber)
    {
        Address = Address.Create(street, city, state, zipCode, country, streetNumber);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateUserData(string newImage, string newBio)
    {
        if (Image == newImage) throw new ImageAlreadyExistsException($"Image {newImage} already exists");

        if (Bio == newBio)
            throw new BioAlreadyExistsException($"Bio {newBio} is the same as previous");

        Bio = newBio;
        Image = newImage;
        UpdatedAt = DateTime.UtcNow;
    }
}