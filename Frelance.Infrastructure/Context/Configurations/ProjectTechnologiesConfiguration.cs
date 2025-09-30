using Frelance.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frelance.Infrastructure.Context.Configurations;


public class ProjectTechnologiesConfiguration : BaseEntityConfiguration<ProjectTechnologies>
{
    public override void Configure(EntityTypeBuilder<ProjectTechnologies> builder)
    {
        base.Configure(builder);
        builder.Property(p => p.Technology).MaxLength100();

        builder.HasOne(pt => pt.Projects)
            .WithMany(p => p.Technologies)
            .HasForeignKey(pt => pt.ProjectId);
    }
}
