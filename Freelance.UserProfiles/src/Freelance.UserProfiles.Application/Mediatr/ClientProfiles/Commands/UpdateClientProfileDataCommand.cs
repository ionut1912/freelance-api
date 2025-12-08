using Shared.Application.Mediator;
namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;

public  record UpdateClientProfileDataCommand(Guid Id,string Bio, string Image): IRequest<Unit>
{
}