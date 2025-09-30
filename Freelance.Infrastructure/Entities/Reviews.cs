namespace Freelance.Infrastructure.Entities;

public class Reviews : BaseEntity
{
    public int Id { get; init; }
    public int ReviewerId { get; set; }
    public Users? Reviewer { get; init; }
    public required string ReviewText { get; set; }
}