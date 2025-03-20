using Frelance.Application.Mediatr.Commands.UserProfile;
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
        IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(userAccessor, nameof(userAccessor));
        ArgumentNullException.ThrowIfNull(freelancerProfilesRepository, nameof(freelancerProfilesRepository));
        ArgumentNullException.ThrowIfNull(userRepository, nameof(userRepository));
        ArgumentNullException.ThrowIfNull(addressRepository, nameof(addressRepository));
        ArgumentNullException.ThrowIfNull(skillsRepository, nameof(skillsRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _userAccessor = userAccessor;
        _freelancerProfilesRepository = freelancerProfilesRepository;
        _userRepository = userRepository;
        _addressRepository = addressRepository;
        _skillsRepository = skillsRepository;
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
        var requestSkills = freelancerProfile.Skills.Adapt<List<SkillRequest>>();
        ValidateSkills(skillsInDb, requestSkills);
        await _freelancerProfilesRepository.CreateAsync(freelancerProfile, cancellationToken);
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

    public async Task<PaginatedList<FreelancerProfileDto>> GetAllFreelancerProfilesAsync(
        PaginationParams paginationParams,
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

        return new PaginatedList<FreelancerProfileDto>(items, count, pageNumber,
            pageSize);
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
        if (addresses is null) throw new NotFoundException($"{nameof(Addresses)} is not found");
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
        var freelancerProfile = await _freelancerProfilesRepository.Query()
            .Where(x => x.Id == patchFreelancerDataCommand.Id)
            .Include(x => x.ForeignLanguages)
            .Include(x => x.Skills)
            .FirstOrDefaultAsync(cancellationToken);
        if (freelancerProfile is null)
            throw new NotFoundException(
                $"{nameof(FreelancerProfiles)} with {nameof(FreelancerProfiles.Id)} : '{patchFreelancerDataCommand.Id}' does not exist");

        foreach (var freelancerLanguage in patchFreelancerDataCommand.FreelancerProfileData.ForeignLanguages.Select(
                     foreignLanguage => new FreelancerForeignLanguage
                     {
                         Language = foreignLanguage,
                         FreelancerProfileId = patchFreelancerDataCommand.Id
                     }))
            freelancerProfile.ForeignLanguages.Add(freelancerLanguage);

        var skills = patchFreelancerDataCommand.FreelancerProfileData.ProgrammingLanguages.Zip(
            patchFreelancerDataCommand.FreelancerProfileData.Areas,
            (pl, area) => new Skills { ProgrammingLanguage = pl, Area = area }).ToList();
        freelancerProfile.Skills.AddRange(skills);
        freelancerProfile.Experience = patchFreelancerDataCommand.FreelancerProfileData.Experience;
        freelancerProfile.Rate = patchFreelancerDataCommand.FreelancerProfileData.Rate;
        freelancerProfile.PortfolioUrl = patchFreelancerDataCommand.FreelancerProfileData.PortfolioUrl;
        _freelancerProfilesRepository.Update(freelancerProfile);
    }

    public async Task VerifyProfileAsync(int id, CancellationToken cancellationToken)
    {
        var freelancer = await _freelancerProfilesRepository.Query()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (freelancer is null)
            throw new NotFoundException($"{nameof(FreelancerProfiles)} with {nameof(id)}: '{id}' does not exist");
        freelancer.IsVerified = true;
        _freelancerProfilesRepository.Update(freelancer);
    }


    public async Task DeleteFreelancerProfileAsync(int id,
        CancellationToken cancellationToken)
    {
        var freelancerToDelete = await _freelancerProfilesRepository.Query()
            .Where(x => x.Id == id)
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