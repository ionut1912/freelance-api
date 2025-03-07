using Frelance.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Frelance.Infrastructure.Context;

public class FrelanceDbContext : IdentityDbContext<Users, Roles, int>
{
    public FrelanceDbContext(DbContextOptions<FrelanceDbContext> options)
        : base(options)
    {
    }

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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ProjectTasks>()
            .HasOne(u => u.FreelancerProfiles)
            .WithMany(u => u.Tasks)
            .HasForeignKey(u => u.FreelancerProfileId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<ProjectTechnologies>()
            .HasOne(p => p.Projects)
            .WithMany(p => p.Technologies)
            .HasForeignKey(p => p.ProjectId);

        builder.Entity<FreelancerForeignLanguage>()
            .HasOne(fld => fld.FreelancerProfile)
            .WithMany(x => x.ForeignLanguages)
            .HasForeignKey(fld => fld.FreelancerProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ProjectTasks>()
            .HasOne(p => p.Projects)
            .WithMany(p => p.Tasks)
            .HasForeignKey(p => p.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<FreelancerProfiles>()
            .HasOne(x => x.Users)
            .WithOne(x => x.FreelancerProfiles)
            .HasForeignKey<FreelancerProfiles>(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<ClientProfiles>()
            .HasOne(x => x.Users)
            .WithOne(x => x.ClientProfiles)
            .HasForeignKey<ClientProfiles>(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Reviews>()
            .HasOne(r => r.Reviewer)
            .WithMany(r => r.Reviews)
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Proposals>()
            .HasOne(x => x.Project)
            .WithMany(x => x.Proposals)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Proposals>()
            .HasOne(x => x.Proposer)
            .WithMany(x => x.Proposals)
            .HasForeignKey(x => x.ProposerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Proposals>()
            .Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder.Entity<Entities.Contracts>()
            .Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder.Entity<Invoices>()
            .Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder.Entity<ClientProfiles>()
            .Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder.Entity<FreelancerProfiles>()
            .Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder.Entity<Projects>()
            .Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder.Entity<ProjectTasks>()
            .Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder.Entity<Reviews>()
            .Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder.Entity<TimeLogs>()
            .Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder.Entity<Entities.Contracts>()
            .HasOne(x => x.Project)
            .WithMany(x => x.Contracts)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Entities.Contracts>()
            .HasOne(x => x.Client)
            .WithMany(x => x.Contracts)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Entities.Contracts>()
            .HasOne(x => x.Freelancer)
            .WithMany(x => x.Contracts)
            .HasForeignKey(x => x.FreelancerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Invoices>()
            .HasOne(x => x.Project)
            .WithMany(x => x.Invoices)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Invoices>()
            .HasOne(x => x.Client)
            .WithMany(x => x.Invoices)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Invoices>()
            .HasOne(x => x.Freelancer)
            .WithMany(x => x.Invoices)
            .HasForeignKey(x => x.FreelancerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Roles>()
            .HasData(
                new Roles { Id = 1, Name = "Freelancer", NormalizedName = "FREELANCER" },
                new Roles { Id = 2, Name = "Client", NormalizedName = "CLIENT" }
            );

        builder.Entity<Skills>()
            .HasData(
                new Skills { Id = 1, ProgrammingLanguage = ".NET", Area = "Backend" },
                new Skills { Id = 2, ProgrammingLanguage = "Angular", Area = "Frontend" },
                new Skills { Id = 3, ProgrammingLanguage = "JavaScript", Area = "Frontend" },
                new Skills { Id = 4, ProgrammingLanguage = "React", Area = "Frontend" },
                new Skills { Id = 5, ProgrammingLanguage = "Python", Area = "Backend" },
                new Skills { Id = 6, ProgrammingLanguage = "Java", Area = "Backend" }
            );

        var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
            d => d.ToDateTime(TimeOnly.MinValue),
            d => DateOnly.FromDateTime(d));

        builder.Entity<Entities.Contracts>()
            .Property(c => c.StartDate)
            .HasConversion(dateOnlyConverter);

        builder.Entity<Entities.Contracts>()
            .Property(c => c.EndDate)
            .HasConversion(dateOnlyConverter);

        builder.Entity<Entities.Contracts>()
            .Property(c => c.Amount)
            .HasPrecision(18, 2);

        builder.Entity<Invoices>()
            .Property(i => i.Amount)
            .HasPrecision(18, 2);

        builder.Entity<Projects>()
            .Property(p => p.Budget)
            .HasPrecision(18, 2);

        builder.Entity<Proposals>()
            .Property(p => p.ProposedBudget)
            .HasPrecision(18, 2);

        builder.Entity<FreelancerProfiles>()
            .HasOne(fp => fp.Addresses)
            .WithOne()
            .HasForeignKey<FreelancerProfiles>(fp => fp.AddressId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ClientProfiles>()
            .HasOne(cp => cp.Addresses)
            .WithOne()
            .HasForeignKey<ClientProfiles>(cp => cp.AddressId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}