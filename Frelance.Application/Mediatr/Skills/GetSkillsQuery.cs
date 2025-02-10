using Frelance.Contracts.Dtos;
using MediatR;

namespace Frelance.Application.Mediatr.Skills;

public record GetSkillsQuery() : IRequest<List<SkillDto>>;
