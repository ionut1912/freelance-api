using JetBrains.Annotations;

namespace Freelance.Contracts.Dtos;

[UsedImplicitly]
public record SkillDto(int Id, string ProgrammingLanguage, string Area);