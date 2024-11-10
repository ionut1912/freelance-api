using Frelance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure
{
    public class FrelanceDbContext : DbContext
    {
        public FrelanceDbContext(DbContextOptions<FrelanceDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        
    }
}