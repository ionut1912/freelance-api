
using Frelance.Contracts.Requests.Skills;
using Microsoft.AspNetCore.Http;


namespace Frelance.Contracts.Requests.FreelancerProfiles;

public class CreateFreelancerProfileRequest
{
    public required string AddressCountry { get; set; }
    public required string AddressStreet { get; set; }
    public required string AddressStreetNumber { get; set; }
    public required string AddressCity { get; set; }
    public required string AddressZip { get; set; }
    public required string Bio { get; set; }

    public required List<string> ProgrammingLanguages { get; set; }

    public required List<string> Areas { get; set; }

    public required List<string> ForeignLanguages { get; set; }
    public required string Experience { get; set; }
    public int Rate { get; set; }
    public required string Currency { get; set; }
    public int Rating { get; set; }
    public required string PortfolioUrl { get; set; }
}