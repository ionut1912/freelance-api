using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Errors;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Requests.FreelancerProfiles;
using Frelance.Contracts.Requests.Skills;
using Frelance.Contracts.Responses.Common;
using Frelance.Infrastructure.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services;

public class FreelancerProfileRepository : IFreelancerProfileRepository
{
    private readonly IGenericRepository<Addresses> _addressRepository;
    private readonly IGenericRepository<FreelancerForeignLanguage> _freelancerForeignLanguageRepository;
    private readonly IGenericRepository<FreelancerProfiles> _freelancerProfilesRepository;
    private readonly IGenericRepository<Skills> _skillsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserAccessor _userAccessor;
    private readonly IGenericRepository<Users> _userRepository;

    public FreelancerProfileRepository(
        IUserAccessor userAccessor,
        IGenericRepository<FreelancerProfiles> freelancerProfilesRepository,
        IGenericRepository<Users> userRepository,
        IGenericRepository<Addresses> addressRepository,
        IGenericRepository<Skills> skillsRepository,
        IGenericRepository<FreelancerForeignLanguage> freelancerForeignLanguageRepository,
        IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(userAccessor, nameof(userAccessor));
        ArgumentNullException.ThrowIfNull(freelancerProfilesRepository, nameof(freelancerProfilesRepository));
        ArgumentNullException.ThrowIfNull(userRepository, nameof(userRepository));
        ArgumentNullException.ThrowIfNull(addressRepository, nameof(addressRepository));
        ArgumentNullException.ThrowIfNull(skillsRepository, nameof(skillsRepository));
        ArgumentNullException.ThrowIfNull(freelancerForeignLanguageRepository,
            nameof(freelancerForeignLanguageRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _userAccessor = userAccessor;
        _freelancerProfilesRepository = freelancerProfilesRepository;
        _userRepository = userRepository;
        _addressRepository = addressRepository;
        _skillsRepository = skillsRepository;
        _freelancerForeignLanguageRepository = freelancerForeignLanguageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task CreateFreelancerProfileAsync(CreateFreelancerProfileRequest createFreelancerProfileRequest,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.Query()
            .Where(x => x.UserName == _userAccessor.GetUsername())
            .FirstOrDefaultAsync(cancellationToken);
        if (user == null) throw new InvalidOperationException("User not found.");

        var freelancerProfile = createFreelancerProfileRequest.Adapt<FreelancerProfiles>();
        freelancerProfile.UserId = user.Id;
        if (freelancerProfile.Addresses is null) throw new NotFoundException("Addresses not found.");

        await _addressRepository.CreateAsync(freelancerProfile.Addresses, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        freelancerProfile.AddressId = freelancerProfile.Addresses.Id;
        var skillsInDb = await _skillsRepository.Query().ToListAsync(cancellationToken);
        var requestSkills = freelancerProfile.Skills?.Adapt<List<SkillRequest>>() ?? [];
        ValidateSkills(skillsInDb, requestSkills);

        try
        {
            await _freelancerProfilesRepository.CreateAsync(freelancerProfile, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            foreach (var fl in freelancerProfile.ForeignLanguages)
            {
                fl.FreelancerProfileId = freelancerProfile.Id;
                fl.Id = 0;
                await _freelancerForeignLanguageRepository.CreateAsync(fl, cancellationToken);
            }
        }
        catch (Exception)
        {
            _freelancerProfilesRepository.Delete(freelancerProfile);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            throw;
        }
    }

    public async Task<FreelancerProfileDto> GetFreelancerProfileByIdAsync(int id,
        CancellationToken cancellationToken)
    {
        var profile = await _freelancerProfilesRepository.Query()
            .Where(x => x.Id == id)
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
            throw new NotFoundException(
                $"{nameof(FreelancerProfiles)} with {nameof(FreelancerProfiles.Id)}: '{id}' does not exist");

        return profile.Adapt<FreelancerProfileDto>();
    }

    public async Task<FreelancerProfileDto> GetLoggedInFreelancerProfileAsync(CancellationToken cancellationToken)
    {
        var profile = await _freelancerProfilesRepository.Query()
            .Where(x => x.Users!.UserName == _userAccessor.GetUsername())
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
        return profile.Adapt<FreelancerProfileDto>();
    }

    public async Task<PaginatedList<FreelancerProfileDto>> GetAllFreelancerProfilesAsync(PaginationParams paginationParams,
        CancellationToken cancellationToken)
    {
        var freelancersProfilesQuery = _freelancerProfilesRepository.Query()
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
            .ProjectToType<FreelancerProfileDto>();

        var count = await freelancersProfilesQuery.CountAsync(cancellationToken);
        var pageNumber = paginationParams.PageNumber;
        var pageSize = paginationParams.PageSize;
        var items = await freelancersProfilesQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<FreelancerProfileDto>(items, count,pageNumber,
            pageSize);
    }

    public async Task UpdateFreelancerProfileAsync(int id, UpdateFreelancerProfileRequest updateFreelancerProfileRequest,
        CancellationToken cancellationToken)
    {
        var freelancerProfile = await _freelancerProfilesRepository.Query()
            .Where(x => x.Id == id)
            .Include(fp => fp.Addresses)
            .Include(fp => fp.Skills)
            .Include(fp => fp.ForeignLanguages)
            .FirstOrDefaultAsync(cancellationToken);
        if (freelancerProfile is null)
            throw new NotFoundException(
                $"{nameof(FreelancerProfiles)} with {nameof(FreelancerProfiles.Id)}: '{id}' does not exist");

        if (updateFreelancerProfileRequest.AddressCountry is not null
            && updateFreelancerProfileRequest.AddressCity is not null
            && updateFreelancerProfileRequest.AddressStreet is not null
            && updateFreelancerProfileRequest.AddressStreetNumber is not null
            && updateFreelancerProfileRequest.AddressZip is not null)
        {
            var updatedAddress = new Addresses
            {
                Id = freelancerProfile.Addresses!.Id,
                Country = updateFreelancerProfileRequest.AddressCountry!,
                City = updateFreelancerProfileRequest.AddressCity,
                Street = updateFreelancerProfileRequest.AddressStreet,
                StreetNumber = updateFreelancerProfileRequest.AddressStreetNumber,
                ZipCode = updateFreelancerProfileRequest.AddressZip
            };
            _addressRepository.Update(freelancerProfile.Addresses);
            freelancerProfile.AddressId = updatedAddress.Id;
        }

        var tempConfig = new TypeAdapterConfig();
        tempConfig.ForType<UpdateFreelancerProfileRequest, FreelancerProfiles>()
            .Ignore(dest => dest.Skills)
            .Ignore(dest => dest.ForeignLanguages)
            .Ignore(dest => dest.Addresses!);
        updateFreelancerProfileRequest.Adapt(freelancerProfile, tempConfig);

        if (updateFreelancerProfileRequest.ProgrammingLanguages is { } progLangs &&
            updateFreelancerProfileRequest.Areas is { } areas)
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

            foreach (var newSkill in newSkills) freelancerProfile.Skills.Add(newSkill);
        }

        if (updateFreelancerProfileRequest.ForeignLanguages is { } newForeignLangs)
        {
            var existingForeignLanguages = await _freelancerForeignLanguageRepository.Query()
                .Where(x => x.FreelancerProfileId ==id)
                .ToListAsync(cancellationToken);

            var languagesToAdd = newForeignLangs
                .Where(lang =>
                    !existingForeignLanguages.Any(x => x.Language.Equals(lang, StringComparison.OrdinalIgnoreCase)))
                .Select(lang => new FreelancerForeignLanguage
                {
                    Language = lang,
                    FreelancerProfileId = id
                })
                .ToList();

            if (languagesToAdd.Count != 0)
            {
                await _freelancerForeignLanguageRepository.CreateRangeAsync(languagesToAdd, cancellationToken);
                freelancerProfile.ForeignLanguages.AddRange(languagesToAdd);
            }
        }

        _freelancerProfilesRepository.Update(freelancerProfile);
    }


    public async Task DeleteFreelancerProfileAsync(int id,
        CancellationToken cancellationToken)
    {
        var freelancerToDelete = await _freelancerProfilesRepository.Query()
            .Where(x => x.Id ==id)
            .FirstOrDefaultAsync(cancellationToken);

        if (freelancerToDelete is null)
            throw new NotFoundException(
                $"{nameof(FreelancerProfiles)} with {nameof(FreelancerProfiles.Id)} : '{id}' does not exist");

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