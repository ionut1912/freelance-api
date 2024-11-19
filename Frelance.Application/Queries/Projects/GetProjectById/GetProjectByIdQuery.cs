using Frelance.Contracts.Responses.Projects;
using MediatR;

namespace Frelance.Application.Queries.Projects.GetProjectById;

public record GetProjectByIdQuery(int Id) : IRequest<GetProjectByIdResponse>;