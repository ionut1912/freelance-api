using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.ValueObjects;

namespace Freelance.Identity.UnitTests.TestData;

internal static class Accounts
{
    public static Account Freelancer(string username = "alice", string password = "hashed") =>
        Account.Create($"{username}@test.io", password, "0700000000", username, Role.Freelancer);

    public static Account Client(string username = "bob") =>
        Account.Create($"{username}@test.io", "hashed", "0700000000", username, Role.Client);
}
