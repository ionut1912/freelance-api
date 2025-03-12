using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Frelance.Contracts.Dtos;

[UsedImplicitly]
[method: SetsRequiredMembers]
public class ClientProfileDto(
    int id,
    UserProfileDto userProfile,
    AddressDto address,
    string bio,
    List<ProjectDto> projects,
    List<ContractsDto> contracts,
    List<InvoicesDto> invoices,
    string image,
    bool isVerified)
    : BaseProfileDto(id, userProfile, address, bio, projects, contracts, invoices, image, isVerified);