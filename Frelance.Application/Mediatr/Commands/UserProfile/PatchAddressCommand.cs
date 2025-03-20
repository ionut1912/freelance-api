using Frelance.Contracts.Dtos;
using Frelance.Contracts.Enums;
using JetBrains.Annotations;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.UserProfile;

[UsedImplicitly]
public record PatchAddressCommand(Role Role, int Id, AddressDto AddressDto) : IRequest;