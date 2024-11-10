using Frelance.API.Frelance.Contracts.Responses;
using MediatR;

namespace Frelance.Application.Queries.Projects.GetProjectById;

public record GetProjectByIdQuery(int Id) : IRequest<GetProjectByIdResponse>;