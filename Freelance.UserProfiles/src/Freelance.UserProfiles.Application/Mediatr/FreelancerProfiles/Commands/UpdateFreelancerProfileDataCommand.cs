using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

public class UpdateFreelancerProfileDataCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Bio { get; set; }
    public string Image { get; set; }
}