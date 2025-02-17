using Frelance.Application.Repositories;
using Frelance.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services;

public class GenericRepository<T>: IGenericRepository<T> where T : class
{
    private readonly FrelanceDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(FrelanceDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext,nameof(dbContext));
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public IQueryable<T> Query()
    {
        return _dbSet.AsNoTracking();
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
       await _dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public void Update(T entity)
    {
       _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
}