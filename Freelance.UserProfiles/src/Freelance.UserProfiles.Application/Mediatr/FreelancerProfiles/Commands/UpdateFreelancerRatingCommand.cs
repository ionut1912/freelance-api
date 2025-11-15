using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

public class UpdateFreelancerRatingCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public int Rating { get; set; }
}