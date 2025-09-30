using Freelance.Application.Repositories;
using Freelance.Infrastructure.Context;

namespace Freelance.Infrastructure.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly FreelanceDbContext _context;

    public UnitOfWork(FreelanceDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        _context = context;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}