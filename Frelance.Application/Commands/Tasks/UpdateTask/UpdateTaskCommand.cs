using Frelance.API.Frelance.Contracts.Enums;
using MediatR;

namespace Frelance.API.Frelance.Application.Commands.Tasks.UpdateTask;

public record UpdateTaskCommand(int Id,string? Title,string? Description,Status? Status,Priority? Priority) : IRequest<Unit>;
