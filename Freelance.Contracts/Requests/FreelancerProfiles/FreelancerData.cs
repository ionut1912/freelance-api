using JetBrains.Annotations;

namespace Freelance.Contracts.Requests.FreelancerProfiles;

[UsedImplicitly]
public record FreelancerData(    List<string> ProgrammingLanguages,
    List<string> ForeignLanguages,
    string Experience,
    int Rate,
    string Currency,
    string PortfolioUrl);