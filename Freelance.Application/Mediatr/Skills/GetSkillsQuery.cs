using Freelance.Contracts.Dtos;
using MediatR;

namespace Freelance.Application.Mediatr.Skills;

public record GetSkillsQuery : IRequest<List<SkillDto>>;