using Frelance.Contracts.Responses.Tasks;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.Tasks;

public record GetTaskByIdQuery(int Id): IRequest<GetTaskByIdResponse>;
