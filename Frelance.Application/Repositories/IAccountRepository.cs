using Frelance.Application.Mediatr.Commands.Users;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Frelance.Application.Repositories;

public interface IAccountRepository
{
    Task RegisterAsync(CreateUserCommand createUserCommand);
}