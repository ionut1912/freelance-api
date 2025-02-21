using Frelance.Application.Mediatr.Skills;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Skills;

public class GetSkillsQueryHandler : IRequestHandler<GetSkillsQuery, List<SkillDto>>
{
    private readonly ISkillsRepository _skillRepository;

    public GetSkillsQueryHandler(ISkillsRepository skillRepository)
    {
        ArgumentNullException.ThrowIfNull(skillRepository, nameof(skillRepository));
        _skillRepository = skillRepository;
    }

    public async Task<List<SkillDto>> Handle(GetSkillsQuery request, CancellationToken cancellationToken)
    {
        return await _skillRepository.GetSkillsAsync(cancellationToken);
    }
}