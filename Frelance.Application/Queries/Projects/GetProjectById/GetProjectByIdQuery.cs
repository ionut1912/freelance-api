using Frelance.API.Frelamce.Contracts;
using MediatR;

namespace Frelance.API.Frelance.Application.Queries.Projects.GetProjectById;

public record GetProjectByIdQuery(int Id) : IRequest<GetProjectByIdResponse>;