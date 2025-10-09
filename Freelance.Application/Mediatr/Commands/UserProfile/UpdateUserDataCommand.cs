using Freelance.Contracts.Enums;
using Freelance.Contracts.Requests.Common;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.UserProfile;

public record UpdateUserDataCommand(Role Role,UpdateUserRequest UpdateUserRequest):IRequest;