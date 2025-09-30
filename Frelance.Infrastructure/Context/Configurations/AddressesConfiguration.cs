using Frelance.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frelance.Infrastructure.Context.Configurations;

public class AddressesConfiguration : BaseEntityConfiguration<Addresses>
{
    public override void Configure(EntityTypeBuilder<Addresses> builder)
    {
        base.Configure(builder);
        builder.Property(p => p.Country).MaxLength100();
        builder.Property(p => p.City).MaxLength100();
        builder.Property(p => p.Street).MaxLength100();
        builder.Property(p => p.StreetNumber).MaxLength100();
        builder.Property(p => p.ZipCode).MaxLength100();
    }
}