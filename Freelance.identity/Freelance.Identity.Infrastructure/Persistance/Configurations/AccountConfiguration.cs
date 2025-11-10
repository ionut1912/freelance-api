using Freelance.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freelance.Identity.Infrastructure.Persistance.Configurations;

public class AccountConfiguration:IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedNever();
        
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
        builder.OwnsOne(p => p.Role, role =>
        {
            role.Property(c => c.Value)
                .HasColumnName("Role")
                .HasMaxLength(20)
                .IsRequired();

            role.HasIndex(c => c.Value)
                .IsUnique();
        });

        builder.Property(a => a.Email)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(a => a.Password)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(a => a.Username)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.IsBlocked)
            .HasDefaultValue(false)
            .IsRequired();
    }
}