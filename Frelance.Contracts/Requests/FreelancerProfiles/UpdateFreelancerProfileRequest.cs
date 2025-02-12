using Microsoft.AspNetCore.Http;

namespace Frelance.Contracts.Requests.FreelancerProfiles;

public class UpdateFreelancerProfileRequest
{
    public string AddressCountry { get; set; }
    public string AddressStreet { get; set; }
    public string AddressStreetNumber { get; set; }
    public string AddressCity { get; set; }
    public string AddressZip { get; set; }
    public string Bio { get; set; }
    public IFormFile ProfileImage { get; set; }

    public List<string> ProgrammingLanguages { get; set; }

    public List<string> Areas { get; set; }

    public List<string> ForeignLanguages { get; set; }
    public string Experience { get; set; }
    public int Rate { get; set; }
    public string Currency { get; set; }
    public int Rating { get; set; }
    public string PortfolioUrl { get; set; }
}