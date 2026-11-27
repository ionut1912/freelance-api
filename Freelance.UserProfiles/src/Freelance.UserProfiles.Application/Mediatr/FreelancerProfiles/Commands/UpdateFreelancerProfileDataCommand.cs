namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

public sealed record UpdateFreelancerProfileDataCommand(Guid Id, string Bio, string Image, Guid AccountId);
