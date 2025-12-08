using Freelance.Identity.Application.Dtos;
using Shared.Application.Mediator;

namespace Freelance.Identity.Application.Mediatr.Accounts.Query;

public record GetCurrentAccountQuery(string Username) : IRequest<AccountDto>
{

}