using JetBrains.Annotations;

namespace Freelance.Contracts.Requests.Skills;

[UsedImplicitly]
public record SkillRequest(string ProgrammingLanguage, string Area);