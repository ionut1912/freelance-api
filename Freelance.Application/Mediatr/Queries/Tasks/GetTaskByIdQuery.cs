using Freelance.Contracts.Dtos;
using MediatR;

namespace Freelance.Application.Mediatr.Queries.Tasks;

public record GetTaskByIdQuery(int Id) : IRequest<TaskDto>;