using JetBrains.Annotations;

namespace Freelance.Contracts.Errors;

[UsedImplicitly]
public record ValidationError(string Property, string ErrorMessage);