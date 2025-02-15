using Frelance.Application.Helpers;
using Frelance.Application.Mediatr.Commands.FreelancerProfiles;
using Frelance.Application.Mediatr.Queries.FreelancerProfiles;
using Frelance.Application.Repositories;
using Frelance.Application.Repositories.External;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Enums;
using Frelance.Contracts.Errors;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Requests.FreelancerProfiles;
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

        public async Task AddFreelancerProfileAsync(CreateFreelancerProfileCommand command, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var freelancerProfile = command.CreateFreelancerProfileRequest.Adapt<FreelancerProfiles>();
            freelancerProfile.UserId = user.Id;

            await _dbContext.Addresses.AddAsync(freelancerProfile.Addresses, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            freelancerProfile.AddressId = freelancerProfile.Addresses.Id;

            freelancerProfile.ProfileImageUrl = await _blobService.UploadBlobAsync(
                StorageContainers.USERIMAGESCONTAINER.ToString().ToLower(),
                $"{user.Id}/{command.CreateFreelancerProfileRequest.ProfileImage.FileName}",
                command.CreateFreelancerProfileRequest.ProfileImage);

            var skillsInDb = await _dbContext.Skills.AsNoTracking().ToListAsync(cancellationToken);
            var requestSkills = freelancerProfile.Skills?.Adapt<List<SkillRequest>>() ?? new List<SkillRequest>();
            ValidateSkills(skillsInDb, requestSkills);

            freelancerProfile.IsAvailable = true;
            try
            {
                await _dbContext.FreelancerProfiles.AddAsync(freelancerProfile, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                if (freelancerProfile.ForeignLanguages != null)
                {
                    foreach (var fl in freelancerProfile.ForeignLanguages)
                    {
                        fl.FreelancerProfileId = freelancerProfile.Id;
                        fl.Id = 0;
                        await _dbContext.FreelancerForeignLanguage.AddAsync(fl, cancellationToken);
                    }
                }

            }
            catch (Exception e)
            {

                _dbContext.FreelancerProfiles.Remove(freelancerProfile);
                await _dbContext.SaveChangesAsync(cancellationToken);
                throw;
            }

        }

        public async Task<FreelancerProfileDto> GetFreelancerProfileByIdAsync(GetFreelancerProfileByIdQuery query, CancellationToken cancellationToken)
        {
            var profile = await _dbContext.Set<FreelancerProfiles>()
                .AsNoTracking()
                .Include(fp => fp.Users)
                    .ThenInclude(u => u.Reviews)
                .Include(fp => fp.Users)
                    .ThenInclude(u => u.Proposals)

                .Include(fp => fp.Contracts)
                .Include(fp => fp.Invoices)
                .Include(fp => fp.Skills)
                .Include(x => x.Addresses)
                .Include(x => x.ForeignLanguages)
                .Include(x => x.Tasks)
                .Include(x => x.Projects)
                .FirstOrDefaultAsync(fp => fp.Id == query.Id, cancellationToken);

            if (profile == null)
            {
                throw new NotFoundException($"{nameof(FreelancerProfiles)} with {nameof(FreelancerProfiles.Id)}: '{query.Id}' does not exist");
            }

            return profile.Adapt<FreelancerProfileDto>();
        }

        public async Task<PaginatedList<FreelancerProfileDto>> GetAllFreelancerProfilesAsync(GetFreelancerProfilesQuery query, CancellationToken cancellationToken)
        {
            var freelancers = _dbContext.FreelancerProfiles
                .AsNoTracking()
                .Include(x => x.Users)
                .ThenInclude(x => x.Reviews)
                .Include(x => x.Users)
                .ThenInclude(x => x.Proposals)
                .Include(x => x.Addresses)
                .Include(f => f.Tasks)
                .Include(x => x.Skills)
                .Include(x => x.ForeignLanguages)
                .Include(x => x.Projects)
                .ProjectToType<FreelancerProfileDto>();

            var count = await freelancers.CountAsync(cancellationToken);
            var items = await freelancers
                .Skip((query.PaginationParams.PageNumber - 1) * query.PaginationParams.PageSize)
                .Take(query.PaginationParams.PageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedList<FreelancerProfileDto>(items, count, query.PaginationParams.PageNumber, query.PaginationParams.PageSize);
        }
        public async Task UpdateFreelancerProfileAsync(UpdateFreelancerProfileCommand command, CancellationToken cancellationToken)
        {
            var freelancerProfile = await _dbContext.FreelancerProfiles
                .Include(fp => fp.Addresses)
                .Include(fp => fp.Skills)
                .Include(fp => fp.ForeignLanguages)
                .FirstOrDefaultAsync(fp => fp.Id == command.Id, cancellationToken);

            if (freelancerProfile == null)
            {
                throw new NotFoundException($"{nameof(FreelancerProfiles)} with {nameof(FreelancerProfiles.Id)}: '{command.Id}' does not exist");
            }

            if (command.UpdateFreelancerProfileRequest.ProfileImage is not null)
            {
                await _blobService.DeleteBlobAsync(
                    StorageContainers.USERIMAGESCONTAINER.ToString().ToLower(),
                    freelancerProfile.UserId.ToString());
                freelancerProfile.ProfileImageUrl = await _blobService.UploadBlobAsync(
                    StorageContainers.USERIMAGESCONTAINER.ToString().ToLower(),
                    $"{freelancerProfile.UserId}/{command.UpdateFreelancerProfileRequest.ProfileImage.FileName}",
                    command.UpdateFreelancerProfileRequest.ProfileImage);
            }

            if (command.UpdateFreelancerProfileRequest.AddressCity is not null)
            {
                var updatedAddress = new Addresses(
                    freelancerProfile.Addresses.Id,
                    command.UpdateFreelancerProfileRequest.AddressCountry,
                    command.UpdateFreelancerProfileRequest.AddressCity,
                    command.UpdateFreelancerProfileRequest.AddressStreet,
                    command.UpdateFreelancerProfileRequest.AddressStreetNumber,
                    command.UpdateFreelancerProfileRequest.AddressZip);
                _dbContext.Entry(freelancerProfile.Addresses).CurrentValues.SetValues(updatedAddress);
                freelancerProfile.AddressId = updatedAddress.Id;
            }

            var tempConfig = new TypeAdapterConfig();
            tempConfig.ForType<UpdateFreelancerProfileRequest, FreelancerProfiles>()
                .Ignore(dest => dest.Skills)
                .Ignore(dest => dest.ForeignLanguages)
                .Ignore(dest => dest.Addresses);
            command.UpdateFreelancerProfileRequest.Adapt(freelancerProfile, tempConfig);

            if (command.UpdateFreelancerProfileRequest.ProgrammingLanguages is { } progLangs &&
                command.UpdateFreelancerProfileRequest.Areas is { } areas)
            {
                var skills = Enumerable.Range(0, Math.Min(progLangs.Count, areas.Count))
                    .Select(i => new SkillRequest(progLangs[i], areas[i]))
                    .ToList();

                var skillsInDb = await _dbContext.Skills.AsNoTracking().ToListAsync(cancellationToken);
                ValidateSkills(skillsInDb, skills);

                var existingSkillLanguages = freelancerProfile.Skills
                    .Select(s => s.ProgrammingLanguage)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                var newSkills = skills
                    .Select(s => s.Adapt<Skills>())
                    .Where(s => !existingSkillLanguages.Contains(s.ProgrammingLanguage))
                    .ToList();

                foreach (var newSkill in newSkills)
                {
                    freelancerProfile.Skills.Add(newSkill);
                }
            }

            if (command.UpdateFreelancerProfileRequest.ForeignLanguages is { } newForeignLangs)
            {
                var existingForeignLanguages = await _dbContext.FreelancerForeignLanguage
                    .AsNoTracking()
                    .Where(x => x.FreelancerProfileId == command.Id)
                    .ToListAsync(cancellationToken);

                var languagesToAdd = newForeignLangs
                    .Where(lang => !existingForeignLanguages.Any(x => x.Language.Equals(lang, StringComparison.OrdinalIgnoreCase)))
                    .Select(lang => new FreelancerForeignLanguage
                    {
                        Language = lang,
                        FreelancerProfileId = command.Id
                    })
                    .ToList();

                if (languagesToAdd.Any())
                {
                    await _dbContext.FreelancerForeignLanguage.AddRangeAsync(languagesToAdd, cancellationToken);
                    freelancerProfile.ForeignLanguages.AddRange(languagesToAdd);
                }
            }
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


        private static void ValidateSkills(List<Skills> skills, List<SkillRequest> skillRequests)
        {
            var missingSkill = skillRequests.Any(req =>
                !skills.Any(dbSkill =>
                    dbSkill.ProgrammingLanguage.Equals(req.ProgrammingLanguage, StringComparison.OrdinalIgnoreCase) &&
                    dbSkill.Area.Equals(req.Area, StringComparison.OrdinalIgnoreCase)));

            if (!missingSkill) return;
            var validationErrors = new List<ValidationError>
            {
                new(nameof(Skills), "Skill not found.")
            };
            throw new CustomValidationException(validationErrors);
        }
    }
}
