using MediatR;
using Frelance.Contracts.Responses.Tasks;
namespace Frelance.Application.Queries.Tasks.GetTaskById;

public record GetTaskByIdQuery(int Id): IRequest<GetTaskByIdResponse>;
