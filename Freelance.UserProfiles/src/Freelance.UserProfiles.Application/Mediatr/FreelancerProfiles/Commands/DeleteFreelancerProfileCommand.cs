namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

public sealed record DeleteFreelancerProfileCommand(Guid Id, Guid AccountId);
