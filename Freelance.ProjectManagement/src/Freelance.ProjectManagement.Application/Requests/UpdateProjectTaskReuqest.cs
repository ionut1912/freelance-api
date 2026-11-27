namespace Freelance.ProjectManagement.Application.Requests;

public sealed record UpdateProjectTaskReuqest(Guid ProjectId, string Title, string Description, string Status, string Priority);
