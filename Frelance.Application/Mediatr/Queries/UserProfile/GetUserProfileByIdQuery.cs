using Frelance.Contracts.Enums;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.UserProfile;

public record GetUserProfileByIdQuery(Role Role, int Id) : IRequest<object>;