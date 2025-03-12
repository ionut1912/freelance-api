using JetBrains.Annotations;

namespace Frelance.Contracts.Errors;

[UsedImplicitly]
public record ValidationError(string Property, string ErrorMessage);