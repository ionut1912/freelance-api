using JetBrains.Annotations;

namespace Frelance.Infrastructure.Entities;

public class BaseUserProfile(Users? users) : BaseEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public Users? Users { get; } = users;
    public int AddressId { get; set; }
    public Addresses? Addresses { get; set; }
    public required string Bio { get; set; }
    public List<Contracts> Contracts { get; } = [];
    public List<Invoices> Invoices { get; } = [];
    
    public List<Projects>? Projects { get; } = [];
    public required string Image { get; set; }
    public bool IsVerified { get; set; }
}