namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;

public sealed record VerifyClientProfileCommand(Guid AccountId, string ImageUrl);
