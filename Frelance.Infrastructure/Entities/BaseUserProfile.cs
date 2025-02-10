namespace Frelance.Infrastructure.Entities;

public class BaseUserProfile
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public Users Users { get; set; }
    public Addresses Addresses { get; set; }
    public string Bio { get; set; }
    public string ProfileImageUrl { get; set; }
    public List<Contracts> Contracts { get; set; } = [];
    public List<Invoices> Invoices { get; set; }
}