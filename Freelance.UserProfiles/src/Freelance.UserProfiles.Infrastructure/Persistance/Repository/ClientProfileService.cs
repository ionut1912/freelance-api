using Shared.Domain.Interfaces;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Freelance.UserProfiles.Infrastructure.Persistance.Repository;

public class ClientProfileService : IClientProfileRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public ClientProfileService(ApplicationDbContext applicationDbContext, IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(applicationDbContext, nameof(applicationDbContext));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _context = applicationDbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task CreateClientProfileAsync(ClientProfile clientProfile, CancellationToken cancellationToken)
    {
        await _context.ClientProfiles.AddAsync(clientProfile, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateClientProfileAsync(ClientProfile clientProfile, CancellationToken cancellationToken)
    {
        _context.ClientProfiles.Update(clientProfile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteClientProfileAsync(ClientProfile clientProfile, CancellationToken cancellationToken)
    {
        _context.ClientProfiles.Remove(clientProfile);
        return _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<ClientProfile> GetClientProfileByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var clientProfile = await _context.ClientProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return clientProfile == null
            ? throw new ProfileNotFoundException($"Profile with id {id} not found")
            : clientProfile;
    }

    public async Task<List<ClientProfile>> GetClientProfilesAsync(CancellationToken cancellationToken)
    {
        var clientProfiles = await _context.ClientProfiles
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return clientProfiles;
    }

    public async Task<ClientProfile> GetLoggedInFreelancerProfileAsync(Guid accountId,
        CancellationToken cancellationToken)
    {
        var clientProfile = await _context.ClientProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AccountId == accountId, cancellationToken);
        return clientProfile == null
            ? throw new ProfileNotFoundException($"Profile with AccountId {accountId} not found")
            : clientProfile;
    }
}