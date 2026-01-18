using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freelance.ProjectManagement.Infrastructure.Persistance.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.OwnsOne(p => p.Budget, mb =>
        {
            mb.Property(m => m.Amount)
                .HasColumnName("BudgetAmount")
                .IsRequired();
            mb.Property(m => m.Currency)
                .HasColumnName("BudgetCurrency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(p => p.Title)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(50)
            .IsRequired();

        builder.OwnsMany(p => p.Technologies, tech =>
        {
            tech.WithOwner().HasForeignKey("ProjectId");
            tech.Property(t => t.Technology)
                .HasMaxLength(100)
                .IsRequired();
            tech.HasKey("ProjectId", nameof(ProjectTechnology.Technology));
            tech.ToTable("ProjectTechnologies");
        });

        builder.HasMany(f => f.Tasks)
            .WithOne()
            .HasForeignKey("ProjectId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}