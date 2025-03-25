using Frelance.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Frelance.Infrastructure.Context;

public class FrelanceDbContext : IdentityDbContext<Users, Roles, int>
{
    public FrelanceDbContext(DbContextOptions<FrelanceDbContext> options) : base(options)
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
            .HasOne(pt => pt.FreelancerProfiles)
            .WithMany(fp => fp.Tasks)
            .HasForeignKey(pt => pt.FreelancerProfileId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<ProjectTechnologies>()
            .HasOne(pt => pt.Projects)
            .WithMany(p => p.Technologies)
            .HasForeignKey(pt => pt.ProjectId);
        builder.Entity<FreelancerForeignLanguage>()
            .HasOne(ffl => ffl.FreelancerProfile)
            .WithMany(fp => fp.ForeignLanguages)
            .HasForeignKey(ffl => ffl.FreelancerProfileId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<FreelancerProfileSkill>(entity =>
        {
            entity.HasKey(fps => new { fps.FreelancerProfileId, fps.SkillId });
            entity.HasOne(fps => fps.FreelancerProfile)
                .WithMany(fp => fp.FreelancerProfileSkills)
                .HasForeignKey(fps => fps.FreelancerProfileId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(fps => fps.Skill)
                .WithMany(s => s.FreelancerProfileSkills)
                .HasForeignKey(fps => fps.SkillId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<ProjectTasks>()
            .HasOne<Projects>()
            .WithMany(p => p.Tasks)
            .HasForeignKey(pt => pt.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<FreelancerProfiles>()
            .HasOne(fp => fp.Users)
            .WithOne(u => u.FreelancerProfiles)
            .HasForeignKey<FreelancerProfiles>(fp => fp.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<ClientProfiles>()
            .HasOne(cp => cp.Users)
            .WithOne(u => u.ClientProfiles)
            .HasForeignKey<ClientProfiles>(cp => cp.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Reviews>()
            .HasOne(r => r.Reviewer)
            .WithMany(rw => rw.Reviews)
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Proposals>()
            .HasOne(p => p.Project)
            .WithMany(pj => pj.Proposals)
            .HasForeignKey(p => p.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Proposals>()
            .HasOne(p => p.Proposer)
            .WithMany(pr => pr.Proposals)
            .HasForeignKey(p => p.ProposerId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Entities.Contracts>()
            .HasOne(c => c.Project)
            .WithMany(p => p.Contracts)
            .HasForeignKey(c => c.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Entities.Contracts>()
            .HasOne(c => c.Client)
            .WithMany(cp => cp.Contracts)
            .HasForeignKey(c => c.ClientId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Entities.Contracts>()
            .HasOne(c => c.Freelancer)
            .WithMany(fp => fp.Contracts)
            .HasForeignKey(c => c.FreelancerId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Invoices>()
            .HasOne(i => i.Project)
            .WithMany(p => p.Invoices)
            .HasForeignKey(i => i.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Invoices>()
            .HasOne(i => i.Client)
            .WithMany(cp => cp.Invoices)
            .HasForeignKey(i => i.ClientId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Invoices>()
            .HasOne(i => i.Freelancer)
            .WithMany(fp => fp.Invoices)
            .HasForeignKey(i => i.FreelancerId)
            .OnDelete(DeleteBehavior.NoAction);
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
        builder.Entity<Proposals>()
            .Property(p => p.UpdatedAt)
            .IsRequired(false);
        builder.Entity<Entities.Contracts>()
            .Property(c => c.UpdatedAt)
            .IsRequired(false);
        builder.Entity<Invoices>()
            .Property(i => i.UpdatedAt)
            .IsRequired(false);
        builder.Entity<ClientProfiles>()
            .Property(cp => cp.UpdatedAt)
            .IsRequired(false);
        builder.Entity<FreelancerProfiles>()
            .Property(fp => fp.UpdatedAt)
            .IsRequired(false);
        builder.Entity<Projects>()
            .Property(p => p.UpdatedAt)
            .IsRequired(false);
        builder.Entity<ProjectTasks>()
            .Property(pt => pt.UpdatedAt)
            .IsRequired(false);
        builder.Entity<Reviews>()
            .Property(r => r.UpdatedAt)
            .IsRequired(false);
        builder.Entity<TimeLogs>()
            .Property(tl => tl.UpdatedAt)
            .IsRequired(false);
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
        builder.Entity<Skills>(entity =>
        {
            entity.Property(p => p.ProgrammingLanguage).HasMaxLength(100);
            entity.Property(p => p.Area).HasMaxLength(100);
        });
        builder.Entity<Reviews>()
            .Property(p => p.ReviewText).HasMaxLength(100);
        builder.Entity<Proposals>()
            .Property(p => p.Status).HasMaxLength(100);
        builder.Entity<ProjectTechnologies>()
            .Property(p => p.Technology).HasMaxLength(100);
        builder.Entity<ProjectTasks>(entity =>
        {
            entity.Property(p => p.Title).HasMaxLength(100);
            entity.Property(p => p.Description).HasMaxLength(100);
            entity.Property(p => p.Status).HasMaxLength(100);
            entity.Property(p => p.Priority).HasMaxLength(100);
        });
        builder.Entity<Projects>(entity =>
        {
            entity.Property(p => p.Title).HasMaxLength(100);
            entity.Property(p => p.Description).HasMaxLength(100);
        });
        builder.Entity<Invoices>(entity =>
        {
            entity.Property(p => p.Status).HasMaxLength(100);
            entity.Property(p => p.InvoiceFile).HasMaxLength(205000);
        });
        builder.Entity<FreelancerProfiles>(entity =>
        {
            entity.Property(p => p.PortfolioUrl).HasMaxLength(100);
            entity.Property(p => p.Currency).HasMaxLength(100);
            entity.Property(p => p.Experience).HasMaxLength(100);
            entity.Property(p => p.Rating).IsRequired(false);
        });
        builder.Entity<FreelancerForeignLanguage>()
            .Property(p => p.Language).HasMaxLength(100);
        builder.Entity<Entities.Contracts>(entity =>
        {
            entity.Property(p => p.Status).HasMaxLength(100);
            entity.Property(p => p.ContractFile).HasMaxLength(205000);
        });
        builder.Entity<Addresses>(entity =>
        {
            entity.Property(p => p.Country).HasMaxLength(100);
            entity.Property(p => p.City).HasMaxLength(100);
            entity.Property(p => p.Street).HasMaxLength(100);
            entity.Property(p => p.StreetNumber).HasMaxLength(100);
            entity.Property(p => p.ZipCode).HasMaxLength(100);
        });
    }
}