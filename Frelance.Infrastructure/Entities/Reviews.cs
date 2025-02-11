namespace Frelance.Infrastructure.Entities;

public class Reviews
{
    public int Id { get; set; }
    public int ReviewerId { get; set; }
    public Users Reviewer { get; set; }
    public string ReviewText { get; set; }
}