using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Infrastructure.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services;

public class SkillRepository : ISkillsRepository
{
    private readonly FrelanceDbContext _context;

    public SkillRepository(FrelanceDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        _context = context;
    }

    public async Task<List<SkillDto>> GetSkillsAsync(CancellationToken ct = default)
    {
        var skill = await _context.Skills.ToListAsync(ct);
        return skill.Adapt<List<SkillDto>>();
    }
}