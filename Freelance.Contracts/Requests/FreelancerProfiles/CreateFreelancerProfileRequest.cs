using Freelance.Contracts.Requests.Common;
using JetBrains.Annotations;

namespace Freelance.Contracts.Requests.FreelancerProfiles;

[UsedImplicitly]
public class CreateFreelancerProfileRequest(
    AddressData address,
    UserData user,
    FreelancerData freelancer
)
{
    public required AddressData Address { get; init; }=address;
    public UserData User { get; }=user;
    public FreelancerData Freelancer { get; }=freelancer;
}