using JetBrains.Annotations;

namespace Frelance.Contracts.Dtos;

[UsedImplicitly]
public record SkillDto(int Id, string ProgrammingLanguage, string Area);