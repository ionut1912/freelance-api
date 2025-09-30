using Freelance.Contracts.Dtos;

namespace Freelance.Application.Repositories;

public interface ISkillsRepository
{
    Task<List<SkillDto>> GetSkillsAsync(CancellationToken ct = default);
}