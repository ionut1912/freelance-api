using JetBrains.Annotations;

namespace Frelance.Infrastructure.Entities;

public class BaseUserProfile(Users? users)
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public Users? Users { get; } = users;
    public int AddressId { get; set; }
    public Addresses? Addresses { get; set; }
    public required string Bio { get; set; }
    public List<Contracts> Contracts { get; } = [];
    public List<Invoices> Invoices { get; } = [];

    [UsedImplicitly] public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public List<Projects>? Projects { get; } = [];
    public required string Image { get; set; }
    public bool IsVerified { get; set; }
}