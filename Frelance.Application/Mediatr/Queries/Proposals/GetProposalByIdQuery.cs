using Frelance.Contracts.Dtos;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.Proposals;

public record GetProposalByIdQuery(int Id) : IRequest<ProposalsDto>;