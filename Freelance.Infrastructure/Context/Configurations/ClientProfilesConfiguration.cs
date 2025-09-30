using Freelance.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freelance.Infrastructure.Context.Configurations;

public class ClientProfilesConfiguration : BaseEntityConfiguration<ClientProfiles>
{
    public override void Configure(EntityTypeBuilder<ClientProfiles> builder)
    {
        base.Configure(builder);

        builder.HasOne(cp => cp.Users)
            .WithOne(u => u.ClientProfiles)
            .HasForeignKey<ClientProfiles>(cp => cp.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(cp => cp.Projects)
            .WithOne(p => p.ClientProfiles)
            .HasForeignKey(p => p.ClientId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(cp => cp.Addresses)
            .WithOne()
            .HasForeignKey<ClientProfiles>(cp => cp.AddressId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}