using Frelance.Contracts.Responses.TimeLogs;
using MediatR;

namespace Frelance.Application.Queries.TimeLogs.GetTimeLogById;

public record GetTimeLogByIdQuery(int Id) : IRequest<GetTimeLogByIdResponse>;