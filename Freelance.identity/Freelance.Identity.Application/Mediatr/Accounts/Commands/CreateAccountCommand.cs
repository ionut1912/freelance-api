using Freelance.Identity.Application.Dtos;
using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.ValueObjects;
using MediatR;

namespace Freelance.Identity.Application.Mediatr.Accounts.Commands;

public class CreateAccountCommand : IRequest<Account>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string Username { get; set; }
    public AddressDto Address { get; set; }
    public string Role { get; set; }
}