using Microsoft.AspNetCore.Http;

namespace Frelance.Contracts.Dtos;

public class ClientProfileDto
{
    public int Id { get; set; }
    public UserProfileDto User { get; set; }
    public AddressDto Address { get; set; }
    public string Bio { get; set; }
    public string ProfileImageUrl { get; set; }
    public List<ContractsDto> Contracts { get; set; }
    public List<ProjectDto>? Projects { get; set; }
    public List<InvoicesDto> Invoices { get; set; }
}
