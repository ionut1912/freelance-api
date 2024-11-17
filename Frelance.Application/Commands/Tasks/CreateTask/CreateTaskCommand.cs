using Frelance.API.Frelance.Contracts.Enums;
using MediatR;

namespace Frelance.API.Frelance.Application.Commands.Tasks.CreateTask;

public record CreateTaskCommand(string ProjectTitle,string Title,string Description,Priority Priority) : IRequest<int>;