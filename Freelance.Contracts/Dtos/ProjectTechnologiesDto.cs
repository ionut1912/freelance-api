using JetBrains.Annotations;

namespace Freelance.Contracts.Dtos;

[UsedImplicitly]
public record ProjectTechnologiesDto(int Id, string Technology,DateTime CreatedAt,DateTime? UpdatedAt);