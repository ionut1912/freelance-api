using Frelance.Contracts.Dtos;

namespace Frelance.Application.Repositories;

public interface ISkillsRepository
{
    Task<List<SkillDto>> GetSkillsAsync(CancellationToken ct = default);
}