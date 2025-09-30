using Frelance.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frelance.Infrastructure.Context.Configurations;

public class FreelancerProfilesConfiguration : BaseEntityConfiguration<FreelancerProfiles>
{
    public override void Configure(EntityTypeBuilder<FreelancerProfiles> builder)
    {
        base.Configure(builder);

        builder.Property(fp => fp.PortfolioUrl).MaxLength100();
        builder.Property(fp => fp.Currency).MaxLength100();
        builder.Property(fp => fp.Experience).MaxLength100();
        builder.Property(fp => fp.Rating).IsRequired(false);

        builder.HasOne(fp => fp.Users)
            .WithOne(u => u.FreelancerProfiles)
            .HasForeignKey<FreelancerProfiles>(fp => fp.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(fp => fp.Projects)
            .WithOne(p => p.FreelancerProfiles)
            .HasForeignKey(p => p.FrelancerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(fp => fp.Addresses)
            .WithOne()
            .HasForeignKey<FreelancerProfiles>(fp => fp.AddressId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}