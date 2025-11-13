using Freelance.UserProfiles.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Freelance.UserProfiles.Infrastructure.Persistance;

public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<ClientProfile> ClientProfiles { get; set; }
    public DbSet<FreelancerProfile>  FreelancerProfiles { get; set; }
    public DbSet<Skill> Skill { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}