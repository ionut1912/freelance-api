using Shared.Domain.Common;

namespace Freelance.ProjectManagement.Domain.ValueObjects;

public class ProjectTechnology : ValueObject
{
    private ProjectTechnology()
    {

    }

    public ProjectTechnology(string technology)
    {
        Technology = technology;
    }

    public string Technology { get; }=string.Empty;

    public static ProjectTechnology Create(string technology)
    {
        return new ProjectTechnology(technology);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Technology;
    }
}
