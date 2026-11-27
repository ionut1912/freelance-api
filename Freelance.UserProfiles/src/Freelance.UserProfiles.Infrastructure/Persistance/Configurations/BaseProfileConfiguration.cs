using Freelance.UserProfiles.Domain.ValueObjects;
using Freelance.UserProfiles.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freelance.UserProfiles.Infrastructure.Persistance.Configurations;

public abstract class BaseProfileConfiguration<TProfile> : IEntityTypeConfiguration<TProfile>
    where TProfile : BaseUserProfile // Assuming both derive from a common base entity (e.g., ProfileBase)
{
    public virtual void Configure(EntityTypeBuilder<TProfile> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.AccountId)
            .IsRequired();

        builder.Property(x => x.Bio)
            .IsRequired()
            .HasMaxLength(BaseUserProfile.BioMaxLength);

        builder.Property(x => x.Image)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.IsVerified)
            .IsRequired()
            .HasDefaultValue(false);

        ConfigureAddress(builder);
    }

    protected void ConfigureAddress(EntityTypeBuilder<TProfile> builder)
    {
        builder.OwnsOne(o => o.Address, address =>
        {
            address.Property(a => a.Street)
                .HasColumnName("Address_Street")
                .HasMaxLength(Address.StreetMaxLength)
                .IsRequired();

            address.Property(a => a.City)
                .HasColumnName("Address_City")
                .HasMaxLength(Address.CityMaxLength)
                .IsRequired();

            address.Property(a => a.State)
                .HasColumnName("Address_State")
                .HasMaxLength(Address.StateMaxLength)
                .IsRequired();

            address.Property(a => a.ZipCode)
                .HasColumnName("Address_ZipCode")
                .HasMaxLength(Address.ZipCodeMaxLength)
                .IsRequired();

            address.Property(a => a.Country)
                .HasColumnName("Address_Country")
                .HasMaxLength(Address.CountryMaxLength)
                .IsRequired();

            address.Property(a => a.StreetNumber)
                .HasColumnName("Address_StreetNumber")
                .HasMaxLength(Address.StreetNumberMaxLength)
                .IsRequired();
        });
    }
}