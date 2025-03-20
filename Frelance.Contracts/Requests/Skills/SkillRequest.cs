using JetBrains.Annotations;

namespace Frelance.Contracts.Requests.Skills;

[UsedImplicitly]
public record SkillRequest(string ProgrammingLanguage, string Area);