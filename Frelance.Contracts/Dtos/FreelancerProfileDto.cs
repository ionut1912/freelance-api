namespace Frelance.Contracts.Dtos;

public record FreelancerProfileDto(
    int Id,
    UserProfileDto UserProfile,
    AddressDto Address,
    string Bio,
    string ProfileImageUrl,
    List<TaskDto> Tasks,
    List<SkillDto> Skills,
    List<ForeignLanguageDto> ForeignLanguages,
    List<ProjectDto>? Projects,
    bool IsAvailable,
    string Experience,
    int Rate,
    string Currency,
    int Rating,
    string PortfolioUrl);
