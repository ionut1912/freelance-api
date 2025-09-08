using JetBrains.Annotations;

namespace Frelance.Contracts.Requests.FreelancerProfiles;

[UsedImplicitly]
public record FreelancerData(    List<string> ProgrammingLanguages,
    List<string> Areas,
    List<string> ForeignLanguages,
    string Experience,
    int Rate,
    string Currency,
    string PortfolioUrl);