namespace Frelance.Infrastructure.Entities;

public class Reviews
{
    public int Id { get; set; }
    public int ReviewerId { get; set; }
    public Users? Reviewer { get; set; }
    public required string ReviewText { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}