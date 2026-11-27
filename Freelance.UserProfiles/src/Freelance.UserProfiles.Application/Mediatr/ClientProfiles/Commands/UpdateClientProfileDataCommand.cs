namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;

public sealed record UpdateClientProfileDataCommand(Guid Id, string Bio, string Image, Guid AccountId);
