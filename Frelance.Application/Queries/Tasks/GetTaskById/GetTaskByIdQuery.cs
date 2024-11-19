using Frelance.Contracts.Responses.Tasks;
using MediatR;

namespace Frelance.Application.Queries.Tasks.GetTaskById;

public record GetTaskByIdQuery(int Id): IRequest<GetTaskByIdResponse>;
