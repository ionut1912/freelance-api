using Shared.Domain.Common;

namespace Freelance.Identity.Domain.ValueObjects;

public class Role : ValueObject
{
    public static readonly Role Freelancer = new("Freelancer");
    public static readonly Role Client = new("Client");

    private Role(string value)
    {
        Value = value;
    }

    public string Value { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}