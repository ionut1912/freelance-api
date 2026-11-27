using System.Diagnostics.CodeAnalysis;
using Shared.Domain.Common;

namespace Freelance.Identity.Domain.ValueObjects;

public sealed class Role : ValueObject
{
    public static readonly Role Freelancer = new("Freelancer");
    public static readonly Role Client = new("Client");

    private static readonly Role[] All = [Freelancer, Client];

    private Role(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static bool IsValid(string? value) => TryFromName(value, out _);

    public static bool TryFromName(string? value, [NotNullWhen(true)] out Role? role)
    {
        role = Array.Find(All, r => string.Equals(r.Value, value, StringComparison.Ordinal));
        return role is not null;
    }

    public static Role FromName(string value) =>
        TryFromName(value, out var role)
            ? role
            : throw new ArgumentOutOfRangeException(nameof(value), value, $"Unknown role '{value}'.");

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
