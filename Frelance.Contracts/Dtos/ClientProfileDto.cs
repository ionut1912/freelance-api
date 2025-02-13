using Microsoft.AspNetCore.Http;

namespace Frelance.Contracts.Dtos;

public record ClientProfileDto(
    int Id,
    UserProfileDto User,
    AddressDto Address,
    string Bio,
    string ProfileImageUrl,
    List<ContractsDto> Contracts,
    List<InvoicesDto> Invoices,
    List<ProjectDto> Projects);
