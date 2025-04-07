using Frelance.Application.Mediatr.Commands.UserProfile;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Errors;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Requests.FreelancerProfiles;
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
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(freelancerForeignLanguageRepository,
            nameof(freelancerForeignLanguageRepository));
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
        if (user == null)
            throw new NotFoundException(
                $"{nameof(Users)} with {nameof(Users.UserName)} :{_userAccessor.GetUsername()} not found.");
        var freelancerProfile = createFreelancerProfileRequest.Adapt<FreelancerProfiles>();
        freelancerProfile.UserId = user.Id;
        if (freelancerProfile.Addresses is null)
            throw new NotFoundException("Addresses not found.");
        await _addressRepository.CreateAsync(freelancerProfile.Addresses, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        freelancerProfile.AddressId = freelancerProfile.Addresses.Id;
        var skillsInDb = await _skillsRepository.Query().ToListAsync(cancellationToken);
        for (var i = 0; i < createFreelancerProfileRequest.ProgrammingLanguages.Count; i++)
        {
            var progLang = createFreelancerProfileRequest.ProgrammingLanguages[i].Trim().ToLowerInvariant();
            var area = createFreelancerProfileRequest.Areas[i].Trim().ToLowerInvariant();
            var existingSkill = skillsInDb.FirstOrDefault(x =>
                x.ProgrammingLanguage.Equals(progLang, StringComparison.InvariantCultureIgnoreCase) &&
                x.Area.Equals(area, StringComparison.InvariantCultureIgnoreCase));
            if (existingSkill == null)
                throw new CustomValidationException([new ValidationError("Skills", "Skill not found.")]);
            freelancerProfile.FreelancerProfileSkills.Add(new FreelancerProfileSkill
                { FreelancerProfileId = freelancerProfile.Id, SkillId = existingSkill.Id });
        }

        await _freelancerProfilesRepository.CreateAsync(freelancerProfile, cancellationToken);
    }

    public async Task<FreelancerProfileDto> GetFreelancerProfileByIdAsync(int id, CancellationToken cancellationToken)
    {
        var profile = await _freelancerProfilesRepository.Query()
            .Where(x => x.Id == id)
            .Include(fp => fp.Users)
            .ThenInclude(u => u!.Reviews)
            .Include(fp => fp.Users)
            .ThenInclude(u => u!.Proposals)
            .Include(fp => fp.Contracts)
            .Include(fp => fp.Invoices)
            .Include(fp => fp.FreelancerProfileSkills)
            .ThenInclude(fps => fps.Skill)
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
        var profile = await _freelancerProfilesRepository.Query().AsTracking()
            .Where(x => x.Users!.UserName == _userAccessor.GetUsername())
            .Include(fp => fp.Users)
            .ThenInclude(u => u!.Reviews)
            .Include(fp => fp.Users)
            .ThenInclude(u => u!.Proposals)
            .Include(fp => fp.Contracts)
            .Include(fp => fp.Invoices)
            .Include(fp => fp.FreelancerProfileSkills)
            .ThenInclude(fps => fps.Skill)
            .Include(x => x.Addresses)
            .Include(x => x.ForeignLanguages)
            .Include(x => x.Tasks)
            .Include(x => x.Projects)
            .FirstOrDefaultAsync(cancellationToken);
        return profile.Adapt<FreelancerProfileDto>();
    }

    public async Task<PaginatedList<FreelancerProfileDto>> GetAllFreelancerProfilesAsync(
        PaginationParams paginationParams, CancellationToken cancellationToken)
    {
        var freelancersProfilesQuery = _freelancerProfilesRepository.Query()
            .Include(fp => fp.Users)
            .ThenInclude(u => u!.Reviews)
            .Include(fp => fp.Users)
            .ThenInclude(u => u!.Proposals)
            .Include(fp => fp.Contracts)
            .Include(fp => fp.Invoices)
            .Include(fp => fp.FreelancerProfileSkills)
            .ThenInclude(fps => fps.Skill)
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
        return new PaginatedList<FreelancerProfileDto>(items, count, pageNumber, pageSize);
    }

    public async Task PatchAddressAsync(PatchAddressCommand patchAddressCommand, CancellationToken cancellationToken)
    {
        var freelancerProfile = await _freelancerProfilesRepository.Query()
            .Where(x => x.Id == patchAddressCommand.Id)
            .Include(x => x.Addresses)
            .FirstOrDefaultAsync(cancellationToken);
        if (freelancerProfile is null)
            throw new NotFoundException(
                $"{nameof(FreelancerProfiles)} with {nameof(FreelancerProfiles.Id)} : '{patchAddressCommand.Id}' does not exist");
        var addresses = freelancerProfile.Addresses;
        if (addresses is null)
            throw new NotFoundException($"{nameof(Addresses)} is not found");
        addresses.Country = patchAddressCommand.AddressDto.Country;
        addresses.City = patchAddressCommand.AddressDto.City;
        addresses.Street = patchAddressCommand.AddressDto.Street;
        addresses.StreetNumber = patchAddressCommand.AddressDto.StreetNumber;
        addresses.ZipCode = patchAddressCommand.AddressDto.ZipCode;
        _addressRepository.Update(addresses);
        freelancerProfile.Addresses = addresses;
        _freelancerProfilesRepository.Update(freelancerProfile);
    }

    public async Task PatchUserDetailsAsync(PatchUserDetailsCommand patchUserDetailsCommand,
        CancellationToken cancellationToken)
    {
        var freelancerProfile = await _freelancerProfilesRepository.Query()
            .Where(x => x.Id == patchUserDetailsCommand.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (freelancerProfile is null)
            throw new NotFoundException(
                $"{nameof(FreelancerProfiles)} with {nameof(FreelancerProfiles.Id)} : '{patchUserDetailsCommand.Id}' does not exist");
        freelancerProfile.Bio = patchUserDetailsCommand.UserDetails.Bio;
        freelancerProfile.Image = patchUserDetailsCommand.UserDetails.Image;
        _freelancerProfilesRepository.Update(freelancerProfile);
    }

    public async Task PatchFreelancerDetailsAsync(PatchFreelancerDataCommand patchFreelancerDataCommand,
        CancellationToken cancellationToken)
    {
        var freelancerProfile = await _freelancerProfilesRepository.Query().AsTracking()
            .Where(x => x.Id == patchFreelancerDataCommand.Id)
            .Include(x => x.ForeignLanguages)
            .Include(x => x.FreelancerProfileSkills)
            .ThenInclude(fps => fps.Skill)
            .FirstOrDefaultAsync(cancellationToken);
        if (freelancerProfile is null)
            throw new NotFoundException(
                $"{nameof(FreelancerProfiles)} with {nameof(FreelancerProfiles.Id)} : '{patchFreelancerDataCommand.Id}' does not exist");

        foreach (var language in patchFreelancerDataCommand.FreelancerProfileData.ForeignLanguages.Distinct(
                     StringComparer.OrdinalIgnoreCase))
        {
            if (freelancerProfile.ForeignLanguages.Any(fl =>
                    fl.Language.Trim().Equals(language.Trim(), StringComparison.CurrentCultureIgnoreCase)))
                continue;
            var foreignLanguage = new FreelancerForeignLanguage
            {
                Language = language,
                FreelancerProfileId = freelancerProfile.Id
            };
            await _freelancerForeignLanguageRepository.CreateAsync(foreignLanguage, cancellationToken);
            freelancerProfile.ForeignLanguages.Add(foreignLanguage);
        }

        for (var i = 0; i < patchFreelancerDataCommand.FreelancerProfileData.ProgrammingLanguages.Count; i++)
        {
            var programmingLanguage = patchFreelancerDataCommand.FreelancerProfileData.ProgrammingLanguages[i].Trim()
                .ToLower();
            var skillArea = await _skillsRepository.Query()
                .Where(x => x.ProgrammingLanguage ==
                            patchFreelancerDataCommand.FreelancerProfileData.ProgrammingLanguages[i])
                .FirstOrDefaultAsync(cancellationToken);
            var area = skillArea!.Area.ToLower();
            if (freelancerProfile.FreelancerProfileSkills.Any(fps =>
                    (fps.Skill?.ProgrammingLanguage ?? "").Trim()
                    .Equals(programmingLanguage, StringComparison.CurrentCultureIgnoreCase) &&
                    (fps.Skill?.Area ?? "").Trim().Equals(area, StringComparison.CurrentCultureIgnoreCase)))
                continue;
            var existingSkill = await _skillsRepository.Query()
                .FirstOrDefaultAsync(x =>
                    x.ProgrammingLanguage.Equals(programmingLanguage, StringComparison.CurrentCultureIgnoreCase) &&
                    x.Area.Equals(area, StringComparison.CurrentCultureIgnoreCase), cancellationToken);
            if (existingSkill != null)
                freelancerProfile.FreelancerProfileSkills.Add(new FreelancerProfileSkill
                    { FreelancerProfileId = freelancerProfile.Id, SkillId = existingSkill.Id });
        }

        freelancerProfile.Experience = patchFreelancerDataCommand.FreelancerProfileData.Experience;
        freelancerProfile.Rate = patchFreelancerDataCommand.FreelancerProfileData.Rate;
        freelancerProfile.Currency = patchFreelancerDataCommand.FreelancerProfileData.Currency;
        freelancerProfile.PortfolioUrl = patchFreelancerDataCommand.FreelancerProfileData.PortfolioUrl;
        _freelancerProfilesRepository.Update(freelancerProfile);
    }


    public async Task VerifyProfileAsync(int id, CancellationToken cancellationToken)
    {
        var freelancer = await _freelancerProfilesRepository.Query()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (freelancer == null)
            throw new NotFoundException($"{nameof(FreelancerProfiles)} with {nameof(id)}: '{id}' does not exist");
        freelancer.IsVerified = true;
        _freelancerProfilesRepository.Update(freelancer);
    }

    public async Task DeleteFreelancerProfileAsync(int id, CancellationToken cancellationToken)
    {
        var freelancerToDelete = await _freelancerProfilesRepository.Query()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (freelancerToDelete == null)
            throw new NotFoundException(
                $"{nameof(FreelancerProfiles)} with {nameof(FreelancerProfiles.Id)} : '{id}' does not exist");
        _freelancerProfilesRepository.Delete(freelancerToDelete);
    }
}