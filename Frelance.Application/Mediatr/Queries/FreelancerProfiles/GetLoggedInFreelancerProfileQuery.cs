using Frelance.Contracts.Dtos;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.FreelancerProfiles;

public record GetLoggedInFreelancerProfileQuery() : IRequest<FreelancerProfileDto?>;