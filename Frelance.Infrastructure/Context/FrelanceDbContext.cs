using Frelance.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Context
{
    public class FrelanceDbContext : IdentityDbContext<Users,Roles,int>
    {
        public FrelanceDbContext(DbContextOptions<FrelanceDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Projects> Projects { get; set; }
        public DbSet<ProjectTasks> Tasks { get; set; }
        public DbSet<TimeLogs> TimeLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ProjectTasks>().HasOne(u => u.Users).WithMany(u => u.Tasks).HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Projects>().HasOne(x=>x.User).WithOne(u=>u.Projects).HasForeignKey<Projects>(x=>x.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<TimeLogs>().HasOne(u=>u.User).WithMany(t=>t.TimeLogs).HasForeignKey(u=>u.UserId).OnDelete(DeleteBehavior.NoAction);
            
        }
    }
}