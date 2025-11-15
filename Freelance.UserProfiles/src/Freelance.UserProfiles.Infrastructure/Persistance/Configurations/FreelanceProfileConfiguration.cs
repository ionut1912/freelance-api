using Freelance.UserProfiles.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freelance.UserProfiles.Infrastructure.Persistance.Configurations;

public class FreelanceProfileConfiguration : BaseProfileConfiguration<FreelancerProfile>
{
    public override void Configure(EntityTypeBuilder<FreelancerProfile> builder)
    {
        base.Configure(builder);


        builder.OwnsOne(f => f.Rate, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("Rate_Amount")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(x => x.Rating)
            .IsRequired(false);

        builder.OwnsMany(f => f.ForeignLanguages, fl =>
        {
            fl.WithOwner().HasForeignKey("FreelancerProfileId");

            fl.Property(l => l.Language)
                .HasColumnName("Language")
                .HasMaxLength(100)
                .IsRequired();
            fl.HasKey("FreelancerProfileId");
            fl.ToTable("FreelancerForeignLanguages");
        });

        builder.HasMany(f => f.Skills)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "FreelancerSkills",
                j => j.HasOne<Skill>().WithMany()
                    .HasForeignKey("SkillId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<FreelancerProfile>().WithMany()
                    .HasForeignKey("FreelancerProfileId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("FreelancerProfileId", "SkillId");
                    j.ToTable("FreelancerSkills");
                });
    }
}