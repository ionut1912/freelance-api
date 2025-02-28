using Frelance.Contracts.Enums;
using Frelance.Contracts.Responses;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Face;

public record VerifyFaceCommand(Role Role,string Base64Face):IRequest<VerifyFaceResult>;