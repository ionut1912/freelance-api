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
        private readonly IBlobService _blobService;
        private readonly IUserAccessor _userAccessor;
        private readonly IGenericRepository<FreelancerProfiles> _freelancerProfilesRepository;
        private readonly IGenericRepository<Users> _userRepository;
        private readonly IGenericRepository<Addresses> _addressRepository;
        private readonly IGenericRepository<Skills> _skillsRepository;
        private readonly IGenericRepository<FreelancerForeignLanguage> _freelancerForeignLanguageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FreelancerProfileRepository(IBlobService blobService,
            IUserAccessor userAccessor,
            IGenericRepository<FreelancerProfiles> freelancerProfilesRepository,
            IGenericRepository<Users> userRepository,
            IGenericRepository<Addresses> addressRepository,
            IGenericRepository<Skills> skillsRepository,
            IGenericRepository<FreelancerForeignLanguage> freelancerForeignLanguageRepository,
            IUnitOfWork unitOfWork)
        {
            ArgumentNullException.ThrowIfNull(blobService, nameof(blobService));
            ArgumentNullException.ThrowIfNull(userAccessor, nameof(userAccessor));
            ArgumentNullException.ThrowIfNull(freelancerProfilesRepository, nameof(freelancerProfilesRepository));
            ArgumentNullException.ThrowIfNull(userRepository, nameof(userRepository));
            ArgumentNullException.ThrowIfNull(addressRepository, nameof(addressRepository));
            ArgumentNullException.ThrowIfNull(skillsRepository, nameof(skillsRepository));
            ArgumentNullException.ThrowIfNull(freelancerForeignLanguageRepository, nameof(freelancerForeignLanguageRepository));
            ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            _blobService = blobService;
            _userAccessor = userAccessor;
            _freelancerProfilesRepository = freelancerProfilesRepository;
            _userRepository = userRepository;
            _addressRepository = addressRepository;
            _skillsRepository = skillsRepository;
            _freelancerForeignLanguageRepository = freelancerForeignLanguageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddFreelancerProfileAsync(CreateFreelancerProfileCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Query()
                .Where(x => x.UserName == _userAccessor.GetUsername())
                .FirstOrDefaultAsync(cancellationToken);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var freelancerProfile = command.CreateFreelancerProfileRequest.Adapt<FreelancerProfiles>();
            freelancerProfile.UserId = user.Id;
            if (freelancerProfile.Addresses is null)
            {
                throw new NotFoundException("Addresses not found.");
            }

            await _addressRepository.AddAsync(freelancerProfile.Addresses, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            freelancerProfile.AddressId = freelancerProfile.Addresses.Id;

            freelancerProfile.ProfileImageUrl = await _blobService.UploadBlobAsync(
                StorageContainers.USERIMAGESCONTAINER.ToString().ToLower(),
                $"{user.Id}/{command.CreateFreelancerProfileRequest.ProfileImage.FileName}",
                command.CreateFreelancerProfileRequest.ProfileImage);

            var skillsInDb = await _skillsRepository.Query().ToListAsync(cancellationToken);
            var requestSkills = freelancerProfile.Skills?.Adapt<List<SkillRequest>>() ?? [];
            ValidateSkills(skillsInDb, requestSkills);

            freelancerProfile.IsAvailable = true;
            try
            {
                await _freelancerProfilesRepository.AddAsync(freelancerProfile, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                if (freelancerProfile.ForeignLanguages != null)
                {
                    foreach (var fl in freelancerProfile.ForeignLanguages)
                    {
                        fl.FreelancerProfileId = freelancerProfile.Id;
                        fl.Id = 0;
                        await _freelancerForeignLanguageRepository.AddAsync(fl, cancellationToken);
                    }
                }

            }
            catch (Exception)
            {

                _freelancerProfilesRepository.Delete(freelancerProfile);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                throw;
            }

        }

        public async Task<FreelancerProfileDto> GetFreelancerProfileByIdAsync(GetFreelancerProfileByIdQuery query, CancellationToken cancellationToken)
        {
            var profile = await _freelancerProfilesRepository.Query()
                .Where(x => x.Id == query.Id)
                .Include(fp => fp.Users)
                .ThenInclude(u => u!.Reviews)
                .Include(fp => fp.Users)
                .ThenInclude(u => u!.Proposals)
                .Include(fp => fp.Contracts)
                .Include(fp => fp.Invoices)
                .Include(fp => fp.Skills)
                .Include(x => x.Addresses)
                .Include(x => x.ForeignLanguages)
                .Include(x => x.Tasks)
                .Include(x => x.Projects)
                .FirstOrDefaultAsync(cancellationToken);

            if (profile == null)
            {
                throw new NotFoundException($"{nameof(FreelancerProfiles)} with {nameof(FreelancerProfiles.Id)}: '{query.Id}' does not exist");
            }

            return profile.Adapt<FreelancerProfileDto>();
        }

        public async Task<PaginatedList<FreelancerProfileDto>> GetAllFreelancerProfilesAsync(GetFreelancerProfilesQuery query, CancellationToken cancellationToken)
        {

            var freelancersProfilesQuery = _freelancerProfilesRepository.Query()
                .Include(x => x.Users)
                .ThenInclude(x => x!.Reviews)
                .Include(x => x.Users)
                .ThenInclude(x => x!.Proposals)
                .Include(x => x.Addresses)
                .Include(f => f.Tasks)
                .Include(x => x.Skills)
                .Include(x => x.ForeignLanguages)
                .Include(x => x.Projects)
                .ProjectToType<FreelancerProfileDto>();

            var count = await freelancersProfilesQuery.CountAsync(cancellationToken);
            var items = await freelancersProfilesQuery
                .Skip((query.PaginationParams.PageNumber - 1) * query.PaginationParams.PageSize)
                .Take(query.PaginationParams.PageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedList<FreelancerProfileDto>(items, count, query.PaginationParams.PageNumber, query.PaginationParams.PageSize);
        }
        public async Task UpdateFreelancerProfileAsync(UpdateFreelancerProfileCommand command, CancellationToken cancellationToken)
        {
            var freelancerProfile = await _freelancerProfilesRepository.Query()
                .Where(x => x.Id == command.Id)
                .Include(fp => fp.Addresses)
                .Include(fp => fp.Skills)
                .Include(fp => fp.ForeignLanguages)
                .FirstOrDefaultAsync(cancellationToken);
            if (freelancerProfile is null)
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

            if (command.UpdateFreelancerProfileRequest.AddressCountry is not null
                && command.UpdateFreelancerProfileRequest.AddressCity is not null
                && command.UpdateFreelancerProfileRequest.AddressStreet is not null
                && command.UpdateFreelancerProfileRequest.AddressStreetNumber is not null
                && command.UpdateFreelancerProfileRequest.AddressZip is not null)
            {

                var updatedAddress = new Addresses
                {
                    Id = freelancerProfile.Addresses!.Id,
                    Country = command.UpdateFreelancerProfileRequest.AddressCountry!,
                    City = command.UpdateFreelancerProfileRequest.AddressCity,
                    Street = command.UpdateFreelancerProfileRequest.AddressStreet,
                    StreetNumber = command.UpdateFreelancerProfileRequest.AddressStreetNumber,
                    ZipCode = command.UpdateFreelancerProfileRequest.AddressZip
                };
                _addressRepository.Update(freelancerProfile.Addresses);
                freelancerProfile.AddressId = updatedAddress.Id;
            }

            var tempConfig = new TypeAdapterConfig();
            tempConfig.ForType<UpdateFreelancerProfileRequest, FreelancerProfiles>()
                .Ignore(dest => dest.Skills)
                .Ignore(dest => dest.ForeignLanguages)
                .Ignore(dest => dest.Addresses!);
            command.UpdateFreelancerProfileRequest.Adapt(freelancerProfile, tempConfig);

            if (command.UpdateFreelancerProfileRequest.ProgrammingLanguages is { } progLangs &&
                command.UpdateFreelancerProfileRequest.Areas is { } areas)
            {
                var skills = Enumerable.Range(0, Math.Min(progLangs.Count, areas.Count))
                    .Select(i => new SkillRequest(progLangs[i], areas[i]))
                    .ToList();

                var skillsInDb = await _skillsRepository.Query().ToListAsync(cancellationToken);
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
                var existingForeignLanguages = await _freelancerForeignLanguageRepository.Query()
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

                if (languagesToAdd.Count != 0)
                {
                    await _freelancerForeignLanguageRepository.AddRangeAsync(languagesToAdd, cancellationToken);
                    freelancerProfile.ForeignLanguages.AddRange(languagesToAdd);
                }
            }
            _freelancerProfilesRepository.Update(freelancerProfile);
        }


        public async Task DeleteFreelancerProfileAsync(DeleteFreelancerProfileCommand deleteFreelancerProfileCommand,
            CancellationToken cancellationToken)
        {
            var freelancerToDelete = await _freelancerProfilesRepository.Query()
                .Where(x => x.Id == deleteFreelancerProfileCommand.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (freelancerToDelete is null)
            {
                throw new NotFoundException(
                    $"{nameof(FreelancerProfiles)} with {nameof(FreelancerProfiles.Id)} : '{deleteFreelancerProfileCommand.Id}' does not exist");
            }

            await _blobService.DeleteBlobAsync(StorageContainers.USERIMAGESCONTAINER.ToString().ToLower(),
                freelancerToDelete.UserId.ToString());
            _freelancerProfilesRepository.Delete(freelancerToDelete);
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
