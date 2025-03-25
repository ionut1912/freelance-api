namespace Frelance.Application.Repositories;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> Query();
    IQueryable<T> QueryTracking();
    Task CreateAsync(T entity, CancellationToken cancellationToken);
    void Update(T entity);
    void Delete(T entity);
}