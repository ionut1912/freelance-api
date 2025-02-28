namespace Frelance.Application.Repositories;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> Query();
    Task CreateAsync(T entity, CancellationToken cancellationToken);
    Task CreateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
    void Update(T entity);
    void Delete(T entity);
}