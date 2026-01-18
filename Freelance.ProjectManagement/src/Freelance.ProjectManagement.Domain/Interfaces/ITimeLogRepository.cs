using Freelance.ProjectManagement.Domain.Entities;
using Shared.Domain.Interfaces;
using System.Linq.Expressions;

namespace Freelance.ProjectManagement.Domain.Interfaces;

public interface ITimeLogRepository : IGenericRepository<TimeLog>
{

    Task<TimeLog?> GetByIdWithTrackingAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<TimeLog, object>>[] includes);
}
