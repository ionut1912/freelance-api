using Shared.Application.Mediator;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

public record UpdateFreelancerDetailsCommand(Guid Id,List<string> ForeignLanguages,List<string> ProgrammingLanguages,List<string> Areas,string Experience,decimal Amount,string Currency,string PortfolioUrl) : IRequest<Unit>
{

}