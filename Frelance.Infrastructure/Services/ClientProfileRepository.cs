using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Requests.ClientProfile;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Responses.Common;
using Frelance.Infrastructure.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services;

public class ClientProfileRepository : IClientProfileRepository
{
    private readonly IGenericRepository<Addresses> _addressRepository;
    private readonly IGenericRepository<ClientProfiles> _clientProfileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserAccessor _userAccessor;
    private readonly IGenericRepository<Users> _userRepository;

    public ClientProfileRepository(IGenericRepository<Users> userRepository,
        IGenericRepository<Addresses> addressRepository,
        IGenericRepository<ClientProfiles> clientProfileRepository,
        IUnitOfWork unitOfWork,
        IUserAccessor userAccessor)
    {
        ArgumentNullException.ThrowIfNull(userRepository, nameof(userRepository));
        ArgumentNullException.ThrowIfNull(addressRepository, nameof(addressRepository));
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(userAccessor, nameof(userAccessor));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _userRepository = userRepository;
        _addressRepository = addressRepository;
        _clientProfileRepository = clientProfileRepository;
        _unitOfWork = unitOfWork;
        _userAccessor = userAccessor;
    }

    public async Task CreateClientProfileAsync(CreateClientProfileRequest createClientProfileRequest,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.Query()
            .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken);
        if (user == null) throw new InvalidOperationException("User not found.");

        var clientProfile = createClientProfileRequest.Adapt<ClientProfiles>();
        if (clientProfile.Addresses is null) throw new NotFoundException("Address not found.");

        await _addressRepository.CreateAsync(clientProfile.Addresses, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        clientProfile.UserId = user.Id;
        clientProfile.AddressId = clientProfile.Addresses.Id;
        await _clientProfileRepository.CreateAsync(clientProfile, cancellationToken);
    }

    public async Task<ClientProfileDto> GetClientProfileByIdAsync(int id,
        CancellationToken cancellationToken)
    {
        var clientProfile = await _clientProfileRepository.Query()
            .Where(x => x.Id ==id)
            .Include(cp => cp.Users)
            .ThenInclude(u => u!.Reviews)
            .Include(cp => cp.Users)
            .ThenInclude(u => u!.Proposals)
            .Include(cp => cp.Contracts)
            .Include(cp => cp.Invoices)
            .Include(x => x.Projects)
            .Include(x => x.Addresses)
            .FirstOrDefaultAsync(cancellationToken);

        if (clientProfile is null)
            throw new NotFoundException(
                $"{nameof(ClientProfiles)} with {nameof(ClientProfiles.Id)} : '{id}' does not exist");

        return clientProfile.Adapt<ClientProfileDto>();
    }

    public async Task<ClientProfileDto> GetLoggedInClientProfileAsync(
        CancellationToken cancellationToken)
    {
        var profile = await _clientProfileRepository.Query()
            .Where(x => x.Users!.UserName == _userAccessor.GetUsername())
            .Include(x => x.Users)
            .ThenInclude(x => x!.Reviews)
            .Include(x => x.Users)
            .ThenInclude(x => x!.Proposals)
            .Include(x => x.Contracts)
            .Include(x => x.Invoices)
            .Include(x => x.Projects)
            .Include(x => x.Addresses)
            .FirstOrDefaultAsync(cancellationToken);
        return profile.Adapt<ClientProfileDto>();
    }

    public async Task<PaginatedList<ClientProfileDto>> GetClientProfilesAsync(PaginationParams paginationParams, CancellationToken cancellationToken)
    {
        var clientsQuery = _clientProfileRepository.Query()
            .Include(x => x.Users)
            .ThenInclude(x => x!.Reviews)
            .Include(x => x.Users)
            .ThenInclude(x => x!.Proposals)
            .Include(x => x.Addresses)
            .Include(x => x.Contracts)
            .Include(x => x.Invoices)
            .Include(x => x.Projects)
            .ProjectToType<ClientProfileDto>();

        var pageNumber = paginationParams.PageNumber;
        var pageSize = paginationParams.PageSize;
        
        var count = await clientsQuery.CountAsync(cancellationToken);
        var items = await clientsQuery
            .Skip((pageNumber - 1) *pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<ClientProfileDto>(items, count,pageNumber,
            pageSize);
    }

    public async Task UpdateClientProfileAsync(int id,UpdateClientProfileRequest updateClientProfileRequest,
        CancellationToken cancellationToken)
    {
        var clientToUpdate = await _clientProfileRepository.Query()
            .Where(x => x.Id ==id)
            .AsNoTracking()
            .Include(x => x.Addresses)
            .FirstOrDefaultAsync(cancellationToken);

        if (clientToUpdate is null)
            throw new NotFoundException(
                $"{nameof(ClientProfiles)} with {nameof(ClientProfiles.Id)} : '{id}' does not exist");
        clientToUpdate = updateClientProfileRequest.Adapt<ClientProfiles>();
        _clientProfileRepository.Update(clientToUpdate);
    }

    public async Task DeleteClientProfileAsync(int id,
        CancellationToken cancellationToken)
    {
        var clientToDelete = await _clientProfileRepository.Query()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (clientToDelete is null)
            throw new NotFoundException(
                $"{nameof(ClientProfiles)} with {nameof(ClientProfiles.Id)} : '{id}' does not exist");
        _clientProfileRepository.Delete(clientToDelete);
    }
}