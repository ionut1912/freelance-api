using Frelance.Application.Helpers;
using Frelance.Application.Mediatr.Commands.ClientProfiles;
using Frelance.Application.Mediatr.Queries.ClientProfiles;
using Frelance.Application.Repositories;
using Frelance.Application.Repositories.External;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Enums;
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

    public async Task AddClientProfileAsync(CreateClientProfileCommand clientProfileCommand, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken);
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        var address = new Addresses(clientProfileCommand.CreateClientProfileRequest.AddressCountry,
            clientProfileCommand.CreateClientProfileRequest.AddressCity, clientProfileCommand.CreateClientProfileRequest.AddressStreet, clientProfileCommand.CreateClientProfileRequest.AddressStreetNumber, clientProfileCommand.CreateClientProfileRequest.AddressZip);
        await _dbContext.Addresses.AddAsync(address, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        var clientProfile = clientProfileCommand.Adapt<ClientProfiles>();
        clientProfile.UserId = user.Id;
        clientProfile.AddressId = address.Id;
        var profileImageUrl = await _blobService.UploadBlobAsync(StorageContainers.USERIMAGESCONTAINER.ToString().ToLower(),
            $"{user.Id}/{clientProfileCommand.CreateClientProfileRequest.ProfileImage.FileName}", clientProfileCommand.CreateClientProfileRequest.ProfileImage);
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
        var clientProfileQueryable = _dbContext.ClientProfiles.ProjectToType<ClientProfileDto>().AsQueryable();
        return await CollectionHelper<ClientProfileDto>.ToPaginatedList(clientProfileQueryable,
                                                                        clientProfilesQuery.PaginationParams.PageNumber,
                                                                        clientProfilesQuery.PaginationParams.PageSize);
    }

    public async Task UpdateClientProfileAsync(UpdateClientProfileCommand clientProfileCommand, CancellationToken cancellationToken)
    {
        var clientToUpdate = await _dbContext.ClientProfiles
                                                         .AsNoTracking()
                                                         .Include(x => x.Addresses)
                                                         .FirstOrDefaultAsync(x => x.Id == clientProfileCommand.Id, cancellationToken);

        if (clientToUpdate is null)
        {
            throw new NotFoundException($"{nameof(ClientProfiles)} with {nameof(ClientProfiles.Id)} : '{clientProfileCommand.Id}' does not exist");
        }

        if (clientProfileCommand.UpdateClientProfileRequest.ProfileImage is not null)
        {
            await _blobService.DeleteBlobAsync(StorageContainers.USERIMAGESCONTAINER.ToString().ToLower(), clientToUpdate.UserId.ToString());
            clientToUpdate.ProfileImageUrl = await _blobService.UploadBlobAsync(StorageContainers.USERIMAGESCONTAINER.ToString().ToLower(),
                $"{clientToUpdate.UserId}/{clientProfileCommand.UpdateClientProfileRequest.ProfileImage.FileName}",
                clientProfileCommand.UpdateClientProfileRequest.ProfileImage);
        }

        if (clientProfileCommand.UpdateClientProfileRequest.AddressCity is not null)
        {
            var address = new Addresses(clientToUpdate.Addresses.Id, clientProfileCommand.UpdateClientProfileRequest.AddressCountry, clientProfileCommand.UpdateClientProfileRequest.AddressCity,
                clientProfileCommand.UpdateClientProfileRequest.AddressStreet, clientProfileCommand.UpdateClientProfileRequest.AddressStreetNumber,
                clientProfileCommand.UpdateClientProfileRequest.AddressZip);
            _dbContext.Entry(clientToUpdate.Addresses).CurrentValues.SetValues(address);
            clientToUpdate.AddressId = address.Id;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        clientToUpdate.Bio = clientProfileCommand.UpdateClientProfileRequest.Bio;
        _dbContext.ClientProfiles.Update(clientToUpdate);
    }

    public async Task DeleteClientProfileAsync(DeleteClientProfileCommand clientProfileCommand, CancellationToken cancellationToken)
    {
        var clientToDelete = await _dbContext.ClientProfiles
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync(x => x.Id == clientProfileCommand.Id, cancellationToken);
        if (clientToDelete is null)
        {
            throw new NotFoundException($"{nameof(ClientProfiles)} with {nameof(ClientProfiles.Id)} : '{clientProfileCommand.Id}' does not exist");
        }

        await _blobService.DeleteBlobAsync(StorageContainers.USERIMAGESCONTAINER.ToString().ToLower(), clientToDelete.UserId.ToString());
        _dbContext.ClientProfiles.Remove(clientToDelete);
    }
}