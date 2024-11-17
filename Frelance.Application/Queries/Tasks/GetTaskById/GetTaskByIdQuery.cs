using Frelance.API.Frelamce.Contracts;
using MediatR;

namespace Frelance.API.Frelance.Application.Queries.Tasks.GetTaskById;

public record GetTaskByIdQuery(int Id): IRequest<GetTaskByIdResponse>;
