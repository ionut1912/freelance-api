using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.ClientProfiles;

public record GetClientProfilesQuery(PaginationParams PaginationParams):IRequest<PaginatedList<ClientProfileDto>>;