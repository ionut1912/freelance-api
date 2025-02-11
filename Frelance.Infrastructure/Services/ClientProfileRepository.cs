using Frelance.Application.Mediatr.Commands.ClientProfiles;
using Frelance.Application.Repositories;
using Frelance.Application.Repositories.External;
using Frelance.Contracts.Dtos;
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
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken);
        var clientProfile = clientProfileCommand.Adapt<ClientProfiles>();
        clientProfile.Users = user;
        var profileImageUrl = await _blobService.UploadBlobAsync("userimagescontainer",
            $"{user.Id}/{clientProfileCommand.ProfileImage.FileName}", clientProfileCommand.ProfileImage);
        clientProfile.ProfileImageUrl = profileImageUrl;
        await _dbContext.ClientProfiles.AddAsync(clientProfile, cancellationToken);
    }
}