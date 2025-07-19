using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;

namespace Frelance.Contracts.Dtos;

[UsedImplicitly]
[method: SetsRequiredMembers]
public class FreelancerProfileDto(
    int id,
    UserProfileDto userProfile,
    AddressDto address,
    string bio,
    List<ProjectDto> projects,
    List<ContractsDto> contracts,
    List<InvoicesDto> invoices,
    string image,
    bool isVerified,
    List<TaskDto> tasks,
    List<SkillDto> skills,
    List<ForeignLanguageDto> foreignLanguage,
    bool isAvailable,
    string experience,
    int rate,
    string currency,
    int rating,
    string portfolioUrl)
    : BaseProfileDto(id, userProfile, address, bio, projects, contracts, invoices, image, isVerified)
{
    [UsedImplicitly] public List<TaskDto> Tasks { get; } = tasks;

    [UsedImplicitly] public List<SkillDto> Skills { get; } = skills;

    [UsedImplicitly] public List<ForeignLanguageDto> ForeignLanguages { get; } = foreignLanguage;

    [UsedImplicitly] public bool IsAvailable { get; } = isAvailable;

    [UsedImplicitly] public required string Experience { get; init; } = experience;

    [UsedImplicitly] public int Rate { get; } = rate;

    [UsedImplicitly] public required string Currency { get; init; } = currency;

    [UsedImplicitly] public int Rating { get; } = rating;

    [UsedImplicitly] public required string PortfolioUrl { get; init; } = portfolioUrl;
}