using JetBrains.Annotations;
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
    [UsedImplicitly] public int Id { get; } = id;

    [UsedImplicitly] public required UserProfileDto User { get; init; } = user;

    [UsedImplicitly] public required AddressDto Address { get; init; } = address;

    [UsedImplicitly] public required string Bio { get; init; } = bio;

    [UsedImplicitly] public List<ProjectDto>? Projects { get; } = projects;

    [UsedImplicitly] public List<ContractsDto>? Contracts { get; } = contracts;

    [UsedImplicitly] public List<InvoicesDto>? Invoices { get; } = invoices;

    [UsedImplicitly] public required string Image { get; init; } = image;

    [UsedImplicitly] public bool IsVerified { get; } = isVerified;
}