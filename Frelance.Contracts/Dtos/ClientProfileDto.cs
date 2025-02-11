using Microsoft.AspNetCore.Http;

namespace Frelance.Contracts.Dtos;

public record ClientProfileDto(
    int Id,
    UserClientDto User,
    AddressDto Address,
    string Bio,
    string ProfileImageUrl,
    List<ContractsDto> Contracts,
    List<InvoicesDto> Invoices);
