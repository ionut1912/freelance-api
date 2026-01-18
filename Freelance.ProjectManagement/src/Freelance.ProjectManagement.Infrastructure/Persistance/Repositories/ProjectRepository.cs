using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Infra.Services;
using System.Linq.Expressions;

namespace Freelance.ProjectManagement.Infrastructure.Persistance.Repositories;

public class ProjectRepository(DbSet<Project> dbSet) : GenericRepository<Project>(dbSet), IProjectRepository
{
    public async Task<Project?> GetByIdWithTrackingAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<Project, object>>[] includes)
    {
        if (includes == null)
        {
            includes = Array.Empty<Expression<Func<Project, object>>>();
        }

        IQueryable<Project> query = dbSet;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
