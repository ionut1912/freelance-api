using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Infrastructure.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services;

public class SkillRepository : ISkillsRepository
{
    private readonly IGenericRepository<Skills> _repository;

    public SkillRepository(IGenericRepository<Skills> repository)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        _repository=repository;
    }

    public async Task<List<SkillDto>> GetSkillsAsync(CancellationToken cancellationToken)
    {
        var skills = await _repository.Query().ToListAsync(cancellationToken);
        return skills.Adapt<List<SkillDto>>();
    }
}