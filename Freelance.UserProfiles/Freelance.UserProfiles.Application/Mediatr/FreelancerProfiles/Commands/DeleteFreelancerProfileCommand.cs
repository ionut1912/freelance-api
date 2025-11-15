using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

public class DeleteFreelancerProfileCommand:IRequest<Unit>
{
    public Guid Id { get; set; }
}