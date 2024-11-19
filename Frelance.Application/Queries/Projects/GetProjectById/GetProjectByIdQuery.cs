using MediatR;
using Frelance.Contracts.Responses.Projects;
namespace Frelance.Application.Queries.Projects.GetProjectById;

public record GetProjectByIdQuery(int Id) : IRequest<GetProjectByIdResponse>;