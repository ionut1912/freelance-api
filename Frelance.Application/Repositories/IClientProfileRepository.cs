using Frelance.Application.Mediatr.Commands.ClientProfiles;
using Frelance.Contracts.Dtos;

namespace Frelance.Application.Repositories;

public interface IClientProfileRepository
{
    Task AddClientProfileAsync(AddClientProfileCommand clientProfileDto, CancellationToken cancellationToken);
}