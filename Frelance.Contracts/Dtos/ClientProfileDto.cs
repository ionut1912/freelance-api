namespace Frelance.Contracts.Dtos;

public class ClientProfileDto
{
    public int Id { get; set; }
    public required UserProfileDto User { get; set; }
    public required AddressDto Address { get; set; }
    public required string Bio { get; set; }
    public List<ContractsDto>? Contracts { get; set; }
    public List<ProjectDto>? Projects { get; set; }
    public List<InvoicesDto>? Invoices { get; set; }
    public required string Image { get; set; }
}