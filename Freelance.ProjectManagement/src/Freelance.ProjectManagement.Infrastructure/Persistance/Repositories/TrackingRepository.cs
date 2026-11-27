using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Common;
using Shared.Infra.Services;

namespace Freelance.ProjectManagement.Infrastructure.Persistance.Repositories;

/// <summary>
/// Extends the shared (no-tracking) generic repository with a tracked lookup, used by command handlers
/// that mutate an aggregate and rely on EF change tracking.
/// </summary>
public abstract class TrackingRepository<T>(DbSet<T> dbSet) : GenericRepository<T>(dbSet) where T : Entity
{
    private readonly DbSet<T> _set = dbSet;

    public Task<T?> GetByIdWithTrackingAsync(Guid id, CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _set;
        foreach (var include in includes ?? [])
            query = query.Include(include);

        return query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
