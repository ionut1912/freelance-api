using Freelance.Identity.Application.Dtos;
using MediatR;

namespace Freelance.Identity.Application.Mediatr.Accounts.Query;

public class LoginQuery : IRequest<AccountDto>
{
    public string Username { get; set; }
    public string Password { get; set; }
}