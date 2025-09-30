using Freelance.Contracts.Dtos;
using MediatR;

namespace Freelance.Application.Mediatr.Queries.Proposals;

public record GetProposalByIdQuery(int Id) : IRequest<ProposalsDto>;