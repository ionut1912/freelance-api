using Frelance.API.Frelance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Frelance.API.Frelance.Infrastructure
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