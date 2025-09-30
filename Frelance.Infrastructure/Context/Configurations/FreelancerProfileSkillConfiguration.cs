using Frelance.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frelance.Infrastructure.Context.Configurations;

public class FreelancerProfileSkillConfiguration : BaseEntityConfiguration<FreelancerProfileSkill>
{
    public override void Configure(EntityTypeBuilder<FreelancerProfileSkill> builder)
    {
        builder.HasKey(fps => new { fps.FreelancerProfileId, fps.SkillId });

        builder.HasOne(fps => fps.FreelancerProfile)
            .WithMany(fp => fp.FreelancerProfileSkills)
            .HasForeignKey(fps => fps.FreelancerProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(fps => fps.Skill)
            .WithMany(s => s.FreelancerProfileSkills)
            .HasForeignKey(fps => fps.SkillId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}