using Freelance.Contracts.Dtos;
using MediatR;

namespace Freelance.Application.Mediatr.Queries.TimeLogs;

public record GetTimeLogByIdQuery(int Id) : IRequest<TimeLogDto>;