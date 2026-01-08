using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Domain.ValueObjects;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class CreateFreelancerProfileCommandHandler : IRequestHandler<CreateFreelancerProfileCommand, FreelancerProfile>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
    private readonly ILogger<CreateFreelancerProfileCommandHandler> _logger;
 
    public CreateFreelancerProfileCommandHandler(IFreelancerProfileRepository freelancerProfileRepository, IUnitOfWork<ApplicationDbContext> unitOfWork,ILogger<CreateFreelancerProfileCommandHandler> logger)
    {
        _freelancerProfileRepository = freelancerProfileRepository ??
                                       throw new ArgumentNullException(nameof(freelancerProfileRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<FreelancerProfile> Handle(CreateFreelancerProfileCommand request,
        CancellationToken cancellationToken)
    {
        var existingFreelancer=await _freelancerProfileRepository.GetLoggedInFreelancerProfileAsync(request.AccountId, cancellationToken);
        if (existingFreelancer != null) 
       {
            _logger.LogError("Freelancer profile creation failed: Profile with accountId {AccountId} already exists", request.AccountId);
            throw new ProfileAllreadyExistsException($"Profile with accountId {request.AccountId} allready exists");
       }

        var freelancerProfile = FreelancerProfile.Create(
            request.AccountId,
            request.Address.Street,
            request.Address.City,
            request.Address.State,
            request.Address.ZipCode,
            request.Address.Country,
            request.Address.StreetNumber,
            request.Bio,
            request.Image,
            request.Experience,
            request.Amount,
            request.Currency,
            0,
            request.PortfolioUrl
        );


        var foreignLanguages = request.ForeignLanguages?
            .Select(language => FreelancerForeignLanguage.Create(language))
            .ToList() ?? [];

        var skills = new List<Skill>();
        if (request.ProgrammingLanguages != null && request.Areas != null)
            for (var i = 0; i < request.ProgrammingLanguages.Count; i++)
            {
                var skill = Skill.Create(request.ProgrammingLanguages[i], request.Areas[i]);
                skills.Add(skill);
            }


        freelancerProfile.AddLanguages(foreignLanguages);
        freelancerProfile.AddSkills(skills);

        await _freelancerProfileRepository.AddAsync(freelancerProfile, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Freelancer profile created successfully for accountId {AccountId}", request.AccountId);
        return freelancerProfile;
    }
}