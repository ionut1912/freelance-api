using Frelance.Contracts.Dtos;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.Tasks;

public record GetTaskByIdQuery(int Id) : IRequest<TaskDto>;