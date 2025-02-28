using Frelance.Contracts.Enums;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.UserProfile;

public record GetCurrentUserProfileQuery(Role Role) : IRequest<object>;