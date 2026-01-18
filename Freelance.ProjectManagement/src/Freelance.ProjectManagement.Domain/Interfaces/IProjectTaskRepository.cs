using Freelance.ProjectManagement.Domain.Entities;
using Shared.Domain.Interfaces;
using System.Linq.Expressions;

namespace Freelance.ProjectManagement.Domain.Interfaces;

public interface IProjectTaskRepository : IGenericRepository<ProjectTask>
{
    Task<ProjectTask?> GetByIdWithTrackingAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<ProjectTask, object>>[] includes);
}
