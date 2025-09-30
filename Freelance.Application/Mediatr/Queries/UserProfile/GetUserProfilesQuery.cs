using Freelance.Contracts.Enums;
using Freelance.Contracts.Requests.Common;
using Freelance.Contracts.Responses.Common;
using MediatR;

namespace Freelance.Application.Mediatr.Queries.UserProfile;

public record GetUserProfilesQuery(Role Role, PaginationParams PaginationParams) : IRequest<PaginatedList<object>>;