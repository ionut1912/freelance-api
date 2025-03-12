namespace Frelance.Infrastructure.Entities;

public class Reviews
{
    public int Id { get; init; }
    public int ReviewerId { get; set; }
    public Users? Reviewer { get; init; }
    public required string ReviewText { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}