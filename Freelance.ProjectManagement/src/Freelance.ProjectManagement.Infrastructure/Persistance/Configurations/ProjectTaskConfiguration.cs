using Freelance.ProjectManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freelance.ProjectManagement.Infrastructure.Persistance.Configurations;

public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder.HasKey(pt => pt.Id);
        builder.Property(pt => pt.Id)
            .ValueGeneratedNever();
        builder.Property(pt => pt.Title)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(pt => pt.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasMany(pt => pt.TimeLogs)
            .WithOne()
            .HasForeignKey("ProjectTaskId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(p => p.Status, mb =>
        {

            mb.Property(m => m.Value)
                .HasColumnName("Status")
                .IsRequired();

        });

        builder.OwnsOne(p => p.Priority, mb =>
        {

            mb.Property(m => m.Value)
                .HasColumnName("Priority")
                .IsRequired();

        });
    }

}
