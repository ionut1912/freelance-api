using Frelance.Contracts.Enums;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.UserProfile;

public record GetUserProfilesQuery(Role Role, PaginationParams PaginationParams) : IRequest<PaginatedList<object>>;
