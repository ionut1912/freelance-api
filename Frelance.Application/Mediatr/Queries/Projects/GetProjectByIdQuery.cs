using Frelance.Contracts.Responses.Projects;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.Projects;

public record GetProjectByIdQuery(int Id) : IRequest<GetProjectByIdResponse>;