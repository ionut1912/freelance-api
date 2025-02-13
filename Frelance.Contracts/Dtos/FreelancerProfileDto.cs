namespace Frelance.Contracts.Dtos;

public record FreelancerProfileDto(
    int Id,
    UserProfileDto UserProfileDto,
    AddressDto AddressDto,
    string Bio,
    string ProfileImageUrl,
    List<TaskDto> TaskDtos,
    List<SkillDto> SkillDtos,
    List<string> ForeignLanguages,
    bool IsAvailable,
    string Experience,
    int Rate,
    string Currency,
    int Rating,
    string PortfolioUrl);
