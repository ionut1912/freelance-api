
using Freelance.ProjectManagement.Domain.Entities;
using Shared.Domain.Interfaces;
using System.Linq.Expressions;

namespace Freelance.ProjectManagement.Domain.Interfaces;

public interface IProjectRepository : IGenericRepository<Project>
{
    Task<Project?> GetByIdWithTrackingAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<Project, object>>[] includes);
}
