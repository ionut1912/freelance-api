using System.Diagnostics.CodeAnalysis;

namespace Frelance.Contracts.Dtos;

[method: SetsRequiredMembers]
public class BaseProfileDto(
    int id,
    UserProfileDto user,
    AddressDto address,
    string bio,
    List<ProjectDto> projects,
    List<ContractsDto> contracts,
    List<InvoicesDto> invoices,
    string image,
    bool isVerified)
{
    public int Id { get; } = id;
    public required UserProfileDto User { get; init; } = user;
    public required AddressDto Address { get; init; } = address;
    public required string Bio { get; init; } = bio;
    public List<ProjectDto>? Projects { get; } = projects;
    public List<ContractsDto>? Contracts { get; } = contracts;
    public List<InvoicesDto>? Invoices { get; } = invoices;
    public required string Image { get; init; } = image;
    public bool IsVerified { get; } = isVerified;
}