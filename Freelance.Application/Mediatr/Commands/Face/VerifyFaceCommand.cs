using Freelance.Contracts.Enums;
using Freelance.Contracts.Responses;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.Face;

public record VerifyFaceCommand(Role Role, string Base64Face) : IRequest<VerifyFaceResult>;