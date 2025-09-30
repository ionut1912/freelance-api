using Freelance.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freelance.Infrastructure.Context.Configurations;

public class ProjectsConfiguration : BaseEntityConfiguration<Projects>
{
    public override void Configure(EntityTypeBuilder<Projects> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Title).MaxLength100();
        builder.Property(p => p.Description).MaxLength100();
        builder.Property(p => p.Budget).HasPrecision(18,2);

        builder.HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(p => p.Technologies)
            .WithOne(pt => pt.Projects)
            .HasForeignKey(pt => pt.ProjectId);
    }
}