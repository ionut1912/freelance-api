namespace Frelance.Infrastructure.Entities;

public class Reviews
{
    public int Id { get; set; }
    public int ReviewerId { get; set; }
    public required Users Reviewer { get; set; }
    public required string ReviewText { get; set; }
}