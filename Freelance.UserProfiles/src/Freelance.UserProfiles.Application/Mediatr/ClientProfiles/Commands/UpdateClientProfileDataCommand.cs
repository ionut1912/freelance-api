using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Commands;

public class UpdateClientProfileDataCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Bio { get; set; }
    public string Image { get; set; }
}