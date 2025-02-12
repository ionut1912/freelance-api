namespace Frelance.Contracts.Dtos;

public record FreelancerProfileDto(
    int Id,
    UserClientDto UserClientDto,
    AddressDto AddressDto,
    string Bio,
    string ProfileImageUrl,
    List<ContractsDto> ContractsDto,
    List<InvoicesDto> InvoicesDto,
    List<ProjectDto> ProjectDtos,
    List<TaskDto> TaskDtos,
    List<SkillDto> SkillDtos,
    List<string> ForeignLanguages,
    bool IsAvailable,
    string Experience,
    int Rate,
    string Currency,
    int Rating,
    string PortfolioUrl);
