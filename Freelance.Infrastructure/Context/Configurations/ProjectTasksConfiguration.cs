using Freelance.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freelance.Infrastructure.Context.Configurations;

public class ProjectTasksConfiguration : BaseEntityConfiguration<ProjectTasks>
{
    public override void Configure(EntityTypeBuilder<ProjectTasks> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Title).MaxLength100();
        builder.Property(p => p.Description).MaxLength100();
        builder.Property(p => p.Status).MaxLength100();
        builder.Property(p => p.Priority).MaxLength100();

        builder.HasOne(pt => pt.FreelancerProfiles)
            .WithMany(fp => fp.Tasks)
            .HasForeignKey(pt => pt.FreelancerProfileId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(pt => pt.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(pt => pt.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}