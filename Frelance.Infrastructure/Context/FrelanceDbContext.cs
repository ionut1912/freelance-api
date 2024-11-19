using Frelance.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Context
{
    public class FrelanceDbContext : DbContext
    {
        public FrelanceDbContext(DbContextOptions<FrelanceDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        
    }
}