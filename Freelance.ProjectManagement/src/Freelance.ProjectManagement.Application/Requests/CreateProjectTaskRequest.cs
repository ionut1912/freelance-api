namespace Freelance.ProjectManagement.Application.Requests;

public sealed record CreateProjectTaskRequest(Guid ProjectId, string Title, string Description, string Status, string Priority);
