using Frelance.Infrastructure.Entities;
using Frelance.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Frelance.Infrastructure.Context
{
    public class FrelanceDbContext : IdentityDbContext<Users, Roles, int>
    {
        public FrelanceDbContext(DbContextOptions<FrelanceDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Projects> Projects { get; set; }
        public DbSet<ProjectTasks> Tasks { get; set; }
        public DbSet<TimeLogs> TimeLogs { get; set; }
        public DbSet<Skiills> Skills { get; set; }
        public DbSet<Addresses> Addresses { get; set; }
        public DbSet<FreelancerProfiles> FreelancerProfiles { get; set; }
        public DbSet<ClientProfiles> ClientProfiles { get; set; }
        public  DbSet<Reviews> Reviews { get; set; }
        public DbSet<Proposals> Proposals { get; set; }
        public DbSet<Entities.Contracts> Contracts { get; set; }
        public DbSet<Invoices> Invoices { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProjectTasks>()
                .HasOne(u => u.FreelancerProfiles)
                .WithMany(u => u.Tasks)
                .HasForeignKey(u => u.FreelancerProfileId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Projects>()
                .HasOne(x => x.FreelancerProfiles)
                .WithMany(u => u.Projects)
                .HasForeignKey(x => x.FreelancerProfileId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<TimeLogs>()
                .HasOne(u => u.FreelancerProfiles)
                .WithMany(t => t.TimeLogs)
                .HasForeignKey(u => u.FreelancerProfileId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<FreelancerProfiles>()
                .HasOne(x=>x.Users)
                .WithOne(x=>x.FreelancerProfiles)
                .HasForeignKey<FreelancerProfiles>(x=>x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<ClientProfiles>()
                .HasOne(x => x.Users)
                .WithOne(x => x.ClientProfiles)
                .HasForeignKey<ClientProfiles>(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Reviews>()
                .HasOne(r=>r.Reviewer)
                .WithMany(r=>r.Reviews)
                .HasForeignKey(r=>r.ReviewerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Proposals>()
                .HasOne(x => x.Project)
                .WithMany(x => x.Proposals)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Proposals>()
                .HasOne(x=>x.Proposer)
                .WithMany(x=>x.Proposals)
                .HasForeignKey(x=>x.ProposerId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Entities.Contracts>()
                .HasOne(x => x.Project)
                .WithMany(x => x.Contracts)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Entities.Contracts>()
                .HasOne(x=>x.Client)
                .WithMany(x=>x.Contracts)
                .HasForeignKey(x=>x.ClientId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Entities.Contracts>()
                .HasOne(x=>x.Freelancer)
                .WithMany(x=>x.Contracts)
                .HasForeignKey(x=>x.FreelancerId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Invoices>()
                .HasOne(x=>x.Project)
                .WithMany(x=>x.Invoices)
                .HasForeignKey(x=>x.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Invoices>()
                .HasOne(x=>x.Client)
                .WithMany(x=>x.Invoices)
                .HasForeignKey(x=>x.ClientId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Invoices>()
                .HasOne(x=>x.Freelancer)
                .WithMany(x=>x.Invoices)
                .HasForeignKey(x=>x.FreelancerId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Roles>()
                .HasData(
                    new Roles { Id = 1, Name = "Freelancer", NormalizedName = "FREELANCER" },
                    new Roles { Id = 2, Name = "Client", NormalizedName = "CLIENT" }
                );
            builder.Entity<Skiills>()
                .HasData(
                    new Skiills { Id = 1, ProgrammingLanguage = ".NET", Area = "Backend" },
                    new Skiills { Id = 2, ProgrammingLanguage = "Angular", Area = "Frontend" },
                    new Skiills { Id = 3, ProgrammingLanguage = "JavaScript", Area = "Frontend" },
                    new Skiills{Id = 4,ProgrammingLanguage = "React", Area = "Frontend" },
                    new Skiills{Id = 5,ProgrammingLanguage = "Python",Area = "Backend"},
                    new Skiills{Id = 6,ProgrammingLanguage = "Java", Area = "Backend"}
                );
        }
    }
}