
using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Infra.Services;
using System.Linq.Expressions;

namespace Freelance.ProjectManagement.Infrastructure.Persistance.Repositories;

public class ProjectTaskRepository(DbSet<ProjectTask> dbSet) : GenericRepository<ProjectTask>(dbSet), IProjectTaskRepository
{
    public async Task<ProjectTask?> GetByIdWithTrackingAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<ProjectTask, object>>[] includes)
    {
        if (includes == null)
        {
            includes = Array.Empty<Expression<Func<ProjectTask, object>>>();
        }

        IQueryable<ProjectTask> query = dbSet;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
