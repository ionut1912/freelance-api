using Frelance.Contracts.Enums;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Tasks;

public record UpdateTaskCommand(int Id,string Title,string Description,ProjectTaskStatus Status,Priority Priority) : IRequest<Unit>;