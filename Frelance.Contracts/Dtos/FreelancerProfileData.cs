using JetBrains.Annotations;

namespace Frelance.Contracts.Dtos;

[UsedImplicitly]
public record FreelancerProfileData(
    List<string> ProgrammingLanguages,
    List<string> Areas,
    List<string> ForeignLanguages,
    string Experience,
    int Rate,
    string Currency,
    string PortfolioUrl);