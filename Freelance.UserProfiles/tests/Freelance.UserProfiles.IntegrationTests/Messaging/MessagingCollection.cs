namespace Freelance.UserProfiles.IntegrationTests.Messaging;

/// <summary>
/// Wolverine tracked sessions observe every message on the (shared) host, so tests that assert on messaging run
/// in a non-parallel collection to avoid seeing each other's activity.
/// </summary>
[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class MessagingCollection
{
    public const string Name = "Messaging";
}
