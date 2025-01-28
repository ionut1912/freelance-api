using Frelance.Infrastructure.Entities;
using Frelance.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Frelance.Infrastructure.Context
{
    public class FrelanceDbContext : IdentityDbContext<Users,Roles,int>
    {
        private readonly DatabaseSettings _databaseSettings;
        public FrelanceDbContext(DbContextOptions<FrelanceDbContext> options,IOptions<DatabaseSettings> databaseSettings) 
            : base(options)
        {
            _databaseSettings = databaseSettings.Value;
        }
        

        public DbSet<Projects> Projects { get; set; }
        public DbSet<ProjectTasks> Tasks { get; set; }
        public DbSet<TimeLogs> TimeLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_databaseSettings.ConnectionString);
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ProjectTasks>().HasOne(u => u.Users).WithMany(u => u.Tasks).HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Projects>().HasOne(x=>x.User).WithOne(u=>u.Projects).HasForeignKey<Projects>(x=>x.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<TimeLogs>().HasOne(u=>u.User).WithMany(t=>t.TimeLogs).HasForeignKey(u=>u.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Roles>()
                .HasData(
                    new Roles { Id = 1, Name = "Frelancer", NormalizedName = "FRELANCER" },
                    new Roles { Id = 2, Name = "Client", NormalizedName = "CLIENT" },
                    new Roles { Id = 3, Name = "Admin", NormalizedName = "ADMIN" }
                );
        }
    }
}