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
        public DbSet<Address> Addresses { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProjectTasks>()
                .HasOne(u => u.Users)
                .WithMany(u => u.Tasks)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Projects>()
                .HasOne(x => x.User)
                .WithMany(u => u.Projects)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<TimeLogs>()
                .HasOne(u => u.User)
                .WithMany(t => t.TimeLogs)
                .HasForeignKey(u => u.UserId)
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