using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

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
    public List<TaskDto> Tasks { get; } = tasks;
    public List<SkillDto> Skills { get; } = skills;
    public List<ForeignLanguageDto> ForeignLanguages { get; } = foreignLanguage;
    public bool IsAvailable { get; } = isAvailable;
    public required string Experience { get; init; } = experience;
    public int Rate { get; } = rate;
    public required string Currency { get; init; } = currency;
    public int Rating { get; } = rating;
    public required string PortfolioUrl { get; init; } = portfolioUrl;
}