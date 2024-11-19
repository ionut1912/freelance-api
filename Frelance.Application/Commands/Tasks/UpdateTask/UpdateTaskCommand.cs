using MediatR;
using Frelance.Contracts.Enums;
namespace Frelance.Application.Commands.Tasks.UpdateTask;

public record UpdateTaskCommand(int Id,string? Title,string? Description,ProjectTaskStatus? Status,Priority? Priority) : IRequest<Unit>;
