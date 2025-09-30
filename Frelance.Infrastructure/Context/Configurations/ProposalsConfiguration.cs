using Frelance.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frelance.Infrastructure.Context.Configurations;

public class ProposalsConfiguration : BaseEntityConfiguration<Proposals>
{
    public override void Configure(EntityTypeBuilder<Proposals> builder)
    {
        base.Configure(builder);
        builder.Property(p => p.Status).MaxLength100();
        builder.Property(p => p.ProposedBudget).HasPrecision(18,2);

        builder.HasOne(p => p.Project)
            .WithMany(pj => pj.Proposals)
            .HasForeignKey(p => p.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(p => p.Proposer)
            .WithMany(fp => fp.Proposals)
            .HasForeignKey(p => p.ProposerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}