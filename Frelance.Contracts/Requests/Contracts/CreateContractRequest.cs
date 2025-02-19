using Microsoft.AspNetCore.Http;

namespace Frelance.Contracts.Requests.Contracts;

public record CreateContractRequest(string ProjectName, string FreelancerName, DateOnly StartDate, DateOnly EndDate, decimal Amount);