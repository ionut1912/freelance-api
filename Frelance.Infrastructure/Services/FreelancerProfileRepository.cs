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

        public async Task AddFreelancerProfileAsync(CreateFreelancerProfileCommand command, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var address = new Addresses(command.CreateFreelancerProfileRequest.AddressCountry, command.CreateFreelancerProfileRequest.AddressCity, command.CreateFreelancerProfileRequest.AddressStreet, command.CreateFreelancerProfileRequest.AddressStreetNumber, command.CreateFreelancerProfileRequest.AddressZip);
            await _dbContext.Addresses.AddAsync(address, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var freelancerProfile = command.Adapt<FreelancerProfiles>();
            freelancerProfile.UserId = user.Id;
            freelancerProfile.AddressId = address.Id;
            freelancerProfile.ProfileImageUrl = await _blobService.UploadBlobAsync(
                StorageContainers.USERIMAGESCONTAINER.ToString().ToLower(),
                $"{user.Id}/{command.CreateFreelancerProfileRequest.ProfileImage.FileName}",
                command.CreateFreelancerProfileRequest.ProfileImage);

            var skillsInDb = await _dbContext.Skills.AsNoTracking().ToListAsync(cancellationToken);
            var skills = new List<SkillRequest>();
            if (command.CreateFreelancerProfileRequest is { ProgrammingLanguages: not null, Areas: not null })
            {
                var count = Math.Min(command.CreateFreelancerProfileRequest.ProgrammingLanguages.Count, command.CreateFreelancerProfileRequest.Areas.Count);
                for (var i = 0; i < count; i++)
                {
                    skills.Add(new SkillRequest(command.CreateFreelancerProfileRequest.ProgrammingLanguages[i], command.CreateFreelancerProfileRequest.Areas[i]));
                }
            }
            ValidateSkills(skillsInDb, skills);
            freelancerProfile.Skills = skills.Adapt<List<Skiills>>();
            freelancerProfile.IsAvailable = true;
            freelancerProfile.Bio = command.CreateFreelancerProfileRequest.Bio;
            freelancerProfile.CreatedAt = DateTime.UtcNow;
            var foreignLanguages = new List<FreelancerForeignLanguage>();
            await _dbContext.FreelancerProfiles.AddAsync(freelancerProfile, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            foreach (var freelancerForeignLanguage in command.CreateFreelancerProfileRequest.ForeignLanguages.Select(language => new FreelancerForeignLanguage
            {
                Language = language,
                FreelancerProfileId = freelancerProfile.Id

            }))
            {
                foreignLanguages.Add(freelancerForeignLanguage);
                await _dbContext.FreelancerForeignLanguage.AddAsync(freelancerForeignLanguage, cancellationToken);
            }

            freelancerProfile.ForeignLanguages = foreignLanguages;

            _dbContext.FreelancerProfiles.Update(freelancerProfile);
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
                .Include(x => x.Addresses)
                .Include(x => x.ForeignLanguages)
                .Include(x => x.Tasks)
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
                .ThenInclude(x => x.Project)
                .Include(x => x.Addresses)
                .Include(f => f.Tasks)
                .Include(x => x.Skills)
                .Include(x => x.ForeignLanguages)
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
                await _blobService.DeleteBlobAsync(StorageContainers.USERIMAGESCONTAINER.ToString().ToLower(), freelancerProfile.UserId.ToString());
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
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            freelancerProfile.Bio = command.UpdateFreelancerProfileRequest.Bio;
            var skills = new List<SkillRequest>();
            if (command.UpdateFreelancerProfileRequest is { ProgrammingLanguages: not null, Areas: not null })
            {
                var count = Math.Min(command.UpdateFreelancerProfileRequest.ProgrammingLanguages.Count, command.UpdateFreelancerProfileRequest.Areas.Count);
                for (var i = 0; i < count; i++)
                {
                    skills.Add(new SkillRequest(command.UpdateFreelancerProfileRequest.ProgrammingLanguages[i], command.UpdateFreelancerProfileRequest.Areas[i]));
                }
            }
            var skillsInDb = await _dbContext.Skills.AsNoTracking().ToListAsync(cancellationToken);
            ValidateSkills(skillsInDb, skills);

            var existingSkillLanguages = freelancerProfile.Skills.Select(s => s.ProgrammingLanguage)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            var newSkills = skills
                .Select(s => s.Adapt<Skiills>())
                .Where(s => !existingSkillLanguages.Contains(s.ProgrammingLanguage))
                .ToList();

            foreach (var newSkill in newSkills)
            {
                freelancerProfile.Skills.Add(newSkill);
            }

            var existingForeignLanguages = await _dbContext.FreelancerForeignLanguage.AsNoTracking()
                .ToListAsync(cancellationToken);


            foreach (var foreignLanguageEntity in from foreignLanguage in command.UpdateFreelancerProfileRequest.ForeignLanguages
                                                  let existingLanguage = existingForeignLanguages.FirstOrDefault(x => x.Language == foreignLanguage)
                                                  where existingLanguage is null
                                                  select new FreelancerForeignLanguage
                                                  { Language = foreignLanguage, FreelancerProfileId = command.Id })
            {
                freelancerProfile.ForeignLanguages.Add(foreignLanguageEntity);
                await _dbContext.FreelancerForeignLanguage.AddAsync(foreignLanguageEntity, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            freelancerProfile.Experience = command.UpdateFreelancerProfileRequest.Experience;
            freelancerProfile.Rate = command.UpdateFreelancerProfileRequest.Rate;
            freelancerProfile.Currency = command.UpdateFreelancerProfileRequest.Currency;
            freelancerProfile.Rating = command.UpdateFreelancerProfileRequest.Rating;
            freelancerProfile.PortfolioUrl = command.UpdateFreelancerProfileRequest.PortfolioUrl;
            freelancerProfile.UpdatedAt = DateTime.UtcNow;
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
