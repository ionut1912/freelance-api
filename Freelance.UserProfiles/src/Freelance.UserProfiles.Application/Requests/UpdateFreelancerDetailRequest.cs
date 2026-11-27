namespace Freelance.UserProfiles.Application.Requests;

public sealed record UpdateFreelancerDetailRequest(List<string> ForeignLanguages,
    List<string> ProgrammingLanguages,
    List<string> Areas,
    string Experience,
    decimal Amount,
    string Currency,
    string PortfolioUrl)
{

}