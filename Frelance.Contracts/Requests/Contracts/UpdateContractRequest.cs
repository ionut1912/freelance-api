using Microsoft.AspNetCore.Http;

namespace Frelance.Contracts.Requests.Contracts;

public class UpdateContractRequest
{
    public DateOnly? EndDate { get; set; }
    public decimal? Amount { get; set; }
    public string? Status { get; set; }
}
