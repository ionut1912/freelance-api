using JetBrains.Annotations;

namespace Freelance.Contracts.Dtos;

[UsedImplicitly]
public record ReviewsDto(int Id, int ReviewerId, string ReviewText, DateTime CreatedAt, DateTime? UpdatedAt);