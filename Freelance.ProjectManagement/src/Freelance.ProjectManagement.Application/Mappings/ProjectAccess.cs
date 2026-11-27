using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mappings;

public static class ProjectAccess
{
    extension(IProjectRepository projectRepository)
    {
        /// <summary>Whether the freelancer is assigned to the project the task belongs to.</summary>
        public async Task<bool> IsAssignedAsync(ProjectTask task, Guid freelancerId, CancellationToken cancellationToken)
        {
            var project = await projectRepository.GetByIdAsync(task.ProjectId, cancellationToken);
            return project?.IsAssignedTo(freelancerId) == true;
        }
    }
}
