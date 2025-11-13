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
            .HasMaxLength(100);

        builder.Property(x => x.Image)
            .IsRequired()
            .HasMaxLength(10000);

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
                .HasMaxLength(200)
                .IsRequired();

            address.Property(a => a.City)
                .HasColumnName("Address_City")
                .HasMaxLength(100)
                .IsRequired();

            address.Property(a => a.State)
                .HasColumnName("Address_State")
                .HasMaxLength(100)
                .IsRequired();

            address.Property(a => a.ZipCode)
                .HasColumnName("Address_ZipCode")
                .HasMaxLength(20)
                .IsRequired();

            address.Property(a => a.Country)
                .HasColumnName("Address_Country")
                .HasMaxLength(100)
                .IsRequired();

            address.Property(a => a.StreetNumber)
                .HasColumnName("Address_StreetNumber")
                .HasMaxLength(100)
                .IsRequired();
        });
    }
}