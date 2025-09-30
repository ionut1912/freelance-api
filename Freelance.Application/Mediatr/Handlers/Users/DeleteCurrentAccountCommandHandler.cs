using Freelance.Application.Mediatr.Commands.Users;
using Freelance.Application.Repositories;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Enums;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.Users;

public class DeleteCurrentAccountCommandHandler : IRequestHandler<DeleteCurrentAccountCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccountRepository _accountRepository;
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;

    public DeleteCurrentAccountCommandHandler(IUnitOfWork unitOfWork, IAccountRepository accountRepository, IClientProfileRepository clientProfileRepository, IFreelancerProfileRepository freelancerProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(accountRepository, nameof(accountRepository));
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        _unitOfWork = unitOfWork;
        _accountRepository = accountRepository;
        _clientProfileRepository = clientProfileRepository;
        _freelancerProfileRepository = freelancerProfileRepository;
    }

    public async Task Handle(DeleteCurrentAccountCommand request, CancellationToken cancellationToken)
    {
        BaseProfileDto? profile = request.Role switch
        {
            Role.Client => await _clientProfileRepository.GetLoggedInClientProfileAsync(cancellationToken),
            Role.Freelancer => await _freelancerProfileRepository.GetLoggedInFreelancerProfileAsync(cancellationToken),
            _ => throw new InvalidOperationException("Invalid request")
        };
        if (request.Role == Role.Client)
                await _clientProfileRepository.DeleteClientProfileAsync(profile.Id, cancellationToken);
        else
                await _freelancerProfileRepository.DeleteFreelancerProfileAsync(profile.Id, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _accountRepository.DeleteCurrentAccountAsync(request, cancellationToken);
    }
}
