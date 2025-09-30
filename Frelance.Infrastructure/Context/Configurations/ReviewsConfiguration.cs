using Frelance.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frelance.Infrastructure.Context.Configurations;

public class ReviewsConfiguration : BaseEntityConfiguration<Reviews>
{
    public override void Configure(EntityTypeBuilder<Reviews> builder)
    {
        base.Configure(builder);
        builder.Property(r => r.ReviewText).MaxLength100();

        builder.HasOne(r => r.Reviewer)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}