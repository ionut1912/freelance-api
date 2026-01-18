using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Infra.Services;
using System.Linq.Expressions;

namespace Freelance.ProjectManagement.Infrastructure.Persistance.Repositories;

public class TimeLogRepository(DbSet<TimeLog> dbSet) : GenericRepository<TimeLog>(dbSet), ITimeLogRepository
{
    public async Task<TimeLog?> GetByIdWithTrackingAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<TimeLog, object>>[] includes)
    {
        if (includes == null)
        {
            includes = Array.Empty<Expression<Func<TimeLog, object>>>();
        }

        IQueryable<TimeLog> query = dbSet;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
