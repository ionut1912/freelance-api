using Frelance.Contracts.Responses.TimeLogs;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.TimeLogs;

public record GetTimeLogByIdQuery(int Id) : IRequest<GetTimeLogByIdResponse>;