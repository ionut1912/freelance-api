using Freelance.Contracts.Enums;
using MediatR;

namespace Freelance.Application.Mediatr.Queries.UserProfile;

public record GetUserProfileByIdQuery(Role Role, int Id) : IRequest<object>;