using Frelance.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frelance.Infrastructure.Context.Configurations;

public class FreelancerForeignLanguageConfiguration : BaseEntityConfiguration<FreelancerForeignLanguage>
{
    public override void Configure(EntityTypeBuilder<FreelancerForeignLanguage> builder)
    {
        base.Configure(builder);
        builder.Property(p => p.Language).MaxLength100();

        builder.HasOne(ffl => ffl.FreelancerProfile)
            .WithMany(fp => fp.ForeignLanguages)
            .HasForeignKey(ffl => ffl.FreelancerProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}