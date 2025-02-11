using Frelance.Application.Helpers;
using Frelance.Application.Mediatr.Commands.ClientProfiles;
using Frelance.Application.Mediatr.Queries.ClientProfiles;
using Frelance.Application.Repositories;
using Frelance.Application.Repositories.External;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Responses.Common;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services;

public class ClientProfileRepository : IClientProfileRepository
{
    private readonly FrelanceDbContext _dbContext;
    private readonly IBlobService _blobService;
    private readonly IUserAccessor _userAccessor;

    public ClientProfileRepository(FrelanceDbContext dbContext, IBlobService blobService, IUserAccessor userAccessor)
    {
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
        ArgumentNullException.ThrowIfNull(blobService, nameof(blobService));
        ArgumentNullException.ThrowIfNull(userAccessor, nameof(userAccessor));
        _dbContext = dbContext;
        _blobService = blobService;
        _userAccessor = userAccessor;
    }
    
    public async Task AddClientProfileAsync(AddClientProfileCommand clientProfileCommand, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken);
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }
        var address = clientProfileCommand.Address.Adapt<Addresses>();
        await _dbContext.Addresses.AddAsync(address, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        var clientProfile = clientProfileCommand.Adapt<ClientProfiles>();
        clientProfile.Users = user;
        clientProfile.AddressId = address.Id;
        var profileImageUrl = await _blobService.UploadBlobAsync("userimagescontainer",
            $"{user.Id}/{clientProfileCommand.ProfileImage.FileName}", clientProfileCommand.ProfileImage);
        clientProfile.ProfileImageUrl = profileImageUrl;
        await _dbContext.ClientProfiles.AddAsync(clientProfile, cancellationToken);
    }

    public async Task<ClientProfileDto> GetClientProfileByIdAsync(GetClientProfileByIdQuery query, CancellationToken cancellationToken)
    {
        var clientProfile = await _dbContext.Set<ClientProfiles>()
            .AsNoTracking()
            .Include(cp => cp.Users)
            .ThenInclude(u => u.Reviews)
            .Include(cp => cp.Users)
            .ThenInclude(u => u.Proposals)
            .ThenInclude(p => p.Project)
            .Include(cp => cp.Addresses)
            .Include(cp => cp.Contracts)
            .ThenInclude(c => c.Project)
            .Include(cp => cp.Invoices)
            .ThenInclude(i => i.Project)
            .FirstOrDefaultAsync(cp => cp.Id == query.Id, cancellationToken);

        if (clientProfile is null)
        {
            throw new NotFoundException($"{nameof(ClientProfiles)} with {nameof(ClientProfiles.Id)} : '{query.Id}' does not exist");
        }

        return clientProfile.Adapt<ClientProfileDto>();
    }

    public async Task<PaginatedList<ClientProfileDto>> GetClientProfilesAsync(GetClientProfilesQuery clientProfilesQuery, CancellationToken cancellationToken)
    {
        var clientProfileQueryable =  _dbContext.ClientProfiles.ProjectToType<ClientProfileDto>().AsQueryable();
        return await CollectionHelper<ClientProfileDto>.ToPaginatedList(clientProfileQueryable, 
                                                                        clientProfilesQuery.PaginationParams.PageNumber, 
                                                                        clientProfilesQuery.PaginationParams.PageSize);
    }
}