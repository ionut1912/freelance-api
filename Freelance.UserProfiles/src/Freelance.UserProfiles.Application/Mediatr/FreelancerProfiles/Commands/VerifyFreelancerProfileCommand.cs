namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

public sealed record VerifyFreelancerProfileCommand(Guid AccountId, string ImageUrl);
