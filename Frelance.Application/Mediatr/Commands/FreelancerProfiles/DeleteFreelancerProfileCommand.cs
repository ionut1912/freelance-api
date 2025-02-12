using MediatR;

namespace Frelance.Application.Mediatr.Commands.FreelancerProfiles;

public record DeleteFreelancerProfileCommand(int Id) : IRequest<Unit>;