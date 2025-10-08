using Freelance.Contracts.Enums;
using Freelance.Contracts.Requests;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.UserProfile;

public record UpdateImageCommand(Role Role,UpdateImageRequest UpdateImageRequest):IRequest;