using Freelance.Identity.Domain.Entities;
using Shared.Application.Mediator;

namespace Freelance.Identity.Application.Mediatr.Accounts.Commands;

public record CreateAccountCommand(string Email,string Password,string PhoneNumber,string Username,string Role) : IRequest<Account>
{

}