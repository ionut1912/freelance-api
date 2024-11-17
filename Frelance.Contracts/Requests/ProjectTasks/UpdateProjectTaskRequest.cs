using Frelance.API.Frelance.Contracts.Enums;

namespace Frelamce.Contracts.Requests.ProjectTasks;

public record UpdateProjectTaskRequest(string ProjectTitle, string Title, string Description, Priority Priority);