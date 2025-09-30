using Freelance.Application.Mediatr.Commands.Users;
using Freelance.Application.Repositories;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.Users;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(accountRepository, nameof(accountRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await _accountRepository.RegisterAsync(request);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}