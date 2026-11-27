using Shared.Domain.Common;

namespace Freelance.ProjectManagement.Domain.ValueObjects;

public sealed class ProjectTechnology : ValueObject
{
    public const int MaxLength = 100;

    private ProjectTechnology() // for EF Core
    {
    }

    public ProjectTechnology(string technology)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(technology);
        Technology = technology;
    }

    public string Technology { get; } = string.Empty;

    public static ProjectTechnology Create(string technology) => new(technology);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Technology;
    }
}
