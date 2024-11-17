using Frelance.API.Frelance.Contracts.Enums;

namespace Frelamce.Contracts.Requests.ProjectTasks;

public record CreateProjectTaskRequest(string ProjectTitle, string Title, string Description, Priority Priority);
