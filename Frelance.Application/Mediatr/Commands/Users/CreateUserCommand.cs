using Frelance.Contracts.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Frelance.Application.Mediatr.Commands.Users;

public record CreateUserCommand(RegisterDto RegisterDto,ModelStateDictionary ModelStateDictionary) : IRequest<Unit>;
