using Frelance.Application.Mediatr.Commands.UserProfile;
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
            .Where(x => x.Id == id)
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

    public async Task<PaginatedList<ClientProfileDto>> GetClientProfilesAsync(PaginationParams paginationParams,
        CancellationToken cancellationToken)
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
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<ClientProfileDto>(items, count, pageNumber,
            pageSize);
    }

    public async Task PatchAddressAsync(PatchAddressCommand patchAddressCommand, CancellationToken cancellationToken)
    {
        var clientProfile = await _clientProfileRepository.Query()
            .Where(x => x.Id == patchAddressCommand.Id)
            .Include(x => x.Addresses)
            .FirstOrDefaultAsync(cancellationToken);
        if (clientProfile is null)
            throw new NotFoundException(
                $"{nameof(ClientProfiles)} with {nameof(ClientProfiles.Id)} : '{patchAddressCommand.Id}' does not exist");
        var addresses = clientProfile.Addresses;
        if (addresses is null) throw new NotFoundException($"{nameof(Addresses)} is not found");

        addresses.Country = patchAddressCommand.AddressDto.Country;
        addresses.City = patchAddressCommand.AddressDto.City;
        addresses.Street = patchAddressCommand.AddressDto.Street;
        addresses.StreetNumber = patchAddressCommand.AddressDto.StreetNumber;
        addresses.ZipCode = patchAddressCommand.AddressDto.ZipCode;
        _addressRepository.Update(addresses);
        clientProfile.Addresses = addresses;
        _clientProfileRepository.Update(clientProfile);
    }

    public async Task PatchUserDetailsAsync(PatchUserDetailsCommand patchUserDetailsCommand,
        CancellationToken cancellationToken)
    {
        var clientProfile = await _clientProfileRepository.Query()
            .Where(x => x.Id == patchUserDetailsCommand.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (clientProfile is null)
            throw new NotFoundException(
                $"{nameof(ClientProfiles)} with {nameof(ClientProfiles.Id)} : '{patchUserDetailsCommand.Id}' does not exist");

        clientProfile.Bio = patchUserDetailsCommand.UserDetails.Bio;
        clientProfile.Image = patchUserDetailsCommand.UserDetails.Image;
        _clientProfileRepository.Update(clientProfile);
    }

    public async Task VerifyProfileAsync(int id, CancellationToken cancellationToken)
    {
        var client = await _clientProfileRepository.Query()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (client is null)
            throw new NotFoundException(
                $"{nameof(ClientProfiles)} with {nameof(ClientProfiles.Id)} : '{id}' does not exist");

        client.IsVerified = true;
        _clientProfileRepository.Update(client);
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