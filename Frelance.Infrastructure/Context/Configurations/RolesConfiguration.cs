using Frelance.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frelance.Infrastructure.Context.Configurations;

public class RolesConfiguration : IEntityTypeConfiguration<Roles>
{
    public void Configure(EntityTypeBuilder<Roles> builder)
    {
        builder.HasData(
            new Roles { Id = 1, Name = "Freelancer", NormalizedName = "FREELANCER" },
            new Roles { Id = 2, Name = "Client", NormalizedName = "CLIENT" }
        );
    }
}
