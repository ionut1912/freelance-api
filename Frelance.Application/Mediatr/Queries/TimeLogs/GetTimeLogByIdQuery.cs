using Frelance.Contracts.Dtos;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.TimeLogs;

public record GetTimeLogByIdQuery(int Id) : IRequest<TimeLogDto>;