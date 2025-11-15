using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Commands;

public class DeleteClientProfileCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}