using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

public class UpdateFreelancerDetailsCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public List<string> ForeignLanguages { get; set; }
    public List<string> ProgrammingLanguages { get; set; }
    public List<string> Areas { get; set; }
    public string Experience { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string PortfolioUrl { get; set; }
}