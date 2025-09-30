using Freelance.Contracts.Enums;
using MediatR;

namespace Freelance.Application.Mediatr.Queries.UserProfile;

public record GetCurrentUserProfileQuery(Role Role) : IRequest<object>;