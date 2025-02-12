using Frelance.Application.Helpers;
using Frelance.Application.Mediatr.Commands.FreelancerProfiles;
using Frelance.Application.Mediatr.Queries.FreelancerProfiles;
using Frelance.Application.Repositories;
using Frelance.Application.Repositories.External;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Enums;
using Frelance.Contracts.Errors;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Requests.Skills;
using Frelance.Contracts.Responses.Common;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services
{
    public class FreelancerProfileRepository : IFreelancerProfileRepository
    {
        private readonly FrelanceDbContext _dbContext;
        private readonly IBlobService _blobService;
        private readonly IUserAccessor _userAccessor;

        public FreelancerProfileRepository(FrelanceDbContext dbContext, IBlobService blobService, IUserAccessor userAccessor)
        {
            ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
            ArgumentNullException.ThrowIfNull(blobService, nameof(blobService));
            ArgumentNullException.ThrowIfNull(userAccessor, nameof(userAccessor));
            _dbContext = dbContext;
            _blobService = blobService;
            _userAccessor = userAccessor;
        }

        public async Task AddFreelancerProfileAsync(AddFreelancerProfileCommand command, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var address = command.Address.Adapt<Addresses>();
            await _dbContext.Addresses.AddAsync(address, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var freelancerProfile = command.Adapt<FreelancerProfiles>();
            freelancerProfile.UserId = user.Id;
            freelancerProfile.AddressId = address.Id;
            freelancerProfile.ProfileImageUrl = await _blobService.UploadBlobAsync(
                StorageContainers.USERIMAGESCONTAINER.ToString().ToLower(),
                $"{user.Id}/{command.ProfileImage.FileName}",
                command.ProfileImage);

            var skillsInDb = await _dbContext.Skills.AsNoTracking().ToListAsync(cancellationToken);
            ValidateSkills(skillsInDb, command.Skills);
            freelancerProfile.Skills = command.Skills.Adapt<List<Skiills>>();
            freelancerProfile.IsAvailable = true;

            await _dbContext.FreelancerProfiles.AddAsync(freelancerProfile, cancellationToken);
        }

        public async Task<FreelancerProfileDto> GetFreelancerProfileByIdAsync(GetFreelancerProfileByIdQuery query, CancellationToken cancellationToken)
        {
            var profile = await _dbContext.Set<FreelancerProfiles>()
                .AsNoTracking()
                .Include(fp => fp.Users)
                    .ThenInclude(u => u.Reviews)
                .Include(fp => fp.Users)
                    .ThenInclude(u => u.Proposals)
                        .ThenInclude(p => p.Project)
                .Include(fp => fp.Contracts)
                    .ThenInclude(c => c.Project)
                .Include(fp => fp.Invoices)
                    .ThenInclude(i => i.Project)
                .Include(fp => fp.Skills)
                .Include(x=>x.Addresses)
                .FirstOrDefaultAsync(fp => fp.Id == query.Id, cancellationToken);

            if (profile == null)
            {
                throw new NotFoundException($"{nameof(FreelancerProfiles)} with {nameof(FreelancerProfiles.Id)}: '{query.Id}' does not exist");
            }

            return profile.Adapt<FreelancerProfileDto>();
        }

        public async Task<PaginatedList<FreelancerProfileDto>> GetAllFreelancerProfilesAsync(GetFreelancerProfilesQuery query, CancellationToken cancellationToken)
        {
            var profilesQuery = _dbContext.FreelancerProfiles
                .ProjectToType<FreelancerProfileDto>()
                .AsQueryable();

            return await CollectionHelper<FreelancerProfileDto>.ToPaginatedList(
                profilesQuery,
                query.PaginationParams.PageNumber,
                query.PaginationParams.PageSize);
        }

public async Task UpdateFreelancerProfileAsync(UpdateFreelancerProfileCommand command, CancellationToken cancellationToken)
{
    var freelancerProfile = await _dbContext.FreelancerProfiles
        .Include(fp => fp.Addresses)
        .Include(fp => fp.Skills)
        .FirstOrDefaultAsync(fp => fp.Id == command.Id, cancellationToken);
    if (freelancerProfile == null)
    {
        throw new NotFoundException($"{nameof(FreelancerProfiles)} with {nameof(FreelancerProfiles.Id)}: '{command.Id}' does not exist");
    }

    if (command.ProfileImage is not null)
    {
        await _blobService.DeleteBlobAsync(StorageContainers.USERIMAGESCONTAINER.ToString().ToLower(),freelancerProfile.UserId.ToString());
        freelancerProfile.ProfileImageUrl = await _blobService.UploadBlobAsync(
            StorageContainers.USERIMAGESCONTAINER.ToString().ToLower(),
            $"{freelancerProfile.UserId}/{command.ProfileImage.FileName}",
            command.ProfileImage);
    }

    if (command.Address is not null)
    {
        var updatedAddress = new Addresses(
            freelancerProfile.Addresses.Id,
            command.Address.Country,
            command.Address.City,
            command.Address.Street,
            command.Address.StreetNumber,
            command.Address.ZipCode);

        _dbContext.Entry(freelancerProfile.Addresses).CurrentValues.SetValues(updatedAddress);
        freelancerProfile.AddressId = updatedAddress.Id;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    freelancerProfile.Bio = command.Bio;

    var skillsInDb = await _dbContext.Skills.AsNoTracking().ToListAsync(cancellationToken);
    ValidateSkills(skillsInDb, command.Skills);

    var existingSkillLanguages = freelancerProfile.Skills.Select(s => s.ProgrammingLanguage)
        .ToHashSet(StringComparer.OrdinalIgnoreCase);
    var newSkills = command.Skills
        .Select(s => s.Adapt<Skiills>())
        .Where(s => !existingSkillLanguages.Contains(s.ProgrammingLanguage))
        .ToList();

    foreach (var newSkill in newSkills)
    {
        freelancerProfile.Skills.Add(newSkill);
    }

    foreach (var foreignLanguage in command.ForeignLanguages.Where(foreignLanguage => !freelancerProfile.ForeignLanguages.Contains(foreignLanguage)))
    {
        freelancerProfile.ForeignLanguages.Add(foreignLanguage);
    }

    freelancerProfile.Experience = command.Experience;
    freelancerProfile.Rate = command.Rate;
    freelancerProfile.Currency = command.Currency;
    freelancerProfile.Rating = command.Rating;
    freelancerProfile.PortfolioUrl = command.PortfolioUrl;
    
    _dbContext.FreelancerProfiles.Update(freelancerProfile);
}

public async Task DeleteFreelancerProfileAsync(DeleteFreelancerProfileCommand deleteFreelancerProfileCommand,
    CancellationToken cancellationToken)
{
    var freelancerToDelete = await _dbContext.FreelancerProfiles
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == deleteFreelancerProfileCommand.Id, cancellationToken);
    if (freelancerToDelete is null)
    {
        throw new NotFoundException(
            $"{nameof(FreelancerProfiles)} with {nameof(FreelancerProfiles.Id)} : '{deleteFreelancerProfileCommand.Id}' does not exist");
    }

    await _blobService.DeleteBlobAsync(StorageContainers.USERIMAGESCONTAINER.ToString().ToLower(),
        freelancerToDelete.UserId.ToString());
    _dbContext.FreelancerProfiles.Remove(freelancerToDelete);
}


private static void ValidateSkills(List<Skiills> skills, List<SkillRequest> skillRequests)
        {
            var missingSkill = skillRequests.Any(req =>
                !skills.Any(dbSkill =>
                    dbSkill.ProgrammingLanguage.Equals(req.ProgrammingLanguage, StringComparison.OrdinalIgnoreCase) &&
                    dbSkill.Area.Equals(req.Area, StringComparison.OrdinalIgnoreCase)));

            if (!missingSkill) return;
            var validationErrors = new List<ValidationError>
            {
                new(nameof(Skiills), "Skill not found.")
            };
            throw new CustomValidationException(validationErrors);
        }
    }
}
