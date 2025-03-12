using JetBrains.Annotations;

[UsedImplicitly]
public record UpdateProjectRequest(
    string? Title,
    string? Description,
    DateTime? Deadline,
    List<string>? Technologies,
    decimal? Budget
);