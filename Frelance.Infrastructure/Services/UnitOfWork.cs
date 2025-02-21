using Frelance.Application.Repositories;
using Frelance.Infrastructure.Context;

namespace Frelance.Infrastructure.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly FrelanceDbContext _context;

    public UnitOfWork(FrelanceDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        _context = context;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}