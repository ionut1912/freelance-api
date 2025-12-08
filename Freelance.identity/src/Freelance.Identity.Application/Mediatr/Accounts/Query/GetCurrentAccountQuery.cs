using Freelance.Identity.Application.Dtos;
using Shared.Application.Mediator;

namespace Freelance.Identity.Application.Mediatr.Accounts.Query;

public class GetCurrentAccountQuery : IRequest<AccountDto>
{
    public string Username { get; set; }
}