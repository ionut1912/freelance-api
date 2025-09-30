using Frelance.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Context;

public class FrelanceDbContext : IdentityDbContext<Users, Roles, int>
{
    public FrelanceDbContext(DbContextOptions<FrelanceDbContext> options) : base(options) { }

    public DbSet<Projects> Projects { get; set; }
    public DbSet<ProjectTasks> Tasks { get; set; }
    public DbSet<TimeLogs> TimeLogs { get; set; }
    public DbSet<Skills> Skills { get; set; }
    public DbSet<Addresses> Addresses { get; set; }
    public DbSet<FreelancerProfiles> FreelancerProfiles { get; set; }
    public DbSet<ClientProfiles> ClientProfiles { get; set; }
    public DbSet<Reviews> Reviews { get; set; }
    public DbSet<Proposals> Proposals { get; set; }
    public DbSet<Entities.Contracts> Contracts { get; set; }
    public DbSet<Invoices> Invoices { get; set; }
    public DbSet<FreelancerForeignLanguage> FreelancerForeignLanguage { get; set; }
    public DbSet<ProjectTechnologies> ProjectTechnologies { get; set; }
    public DbSet<FreelancerProfileSkill> FreelancerProfileSkills { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(FrelanceDbContext).Assembly);
    }
}