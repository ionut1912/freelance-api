using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Freelance.Infrastructure.Context.Configurations;

public class ContractsConfiguration : BaseEntityConfiguration<Entities.Contracts>
{
    public override void Configure(EntityTypeBuilder<Entities.Contracts> builder)
    {
        base.Configure(builder);

        builder.Property(c => c.Status).MaxLength100();
        builder.Property(c => c.ContractFile).MaxLength205000();
        builder.Property(c => c.Amount).HasPrecision(18, 2);

        var dateConverter = new ValueConverter<DateOnly, DateTime>(
            d => d.ToDateTime(TimeOnly.MinValue),
            d => DateOnly.FromDateTime(d));

        builder.Property(c => c.StartDate).HasConversion(dateConverter);
        builder.Property(c => c.EndDate).HasConversion(dateConverter);

        builder.HasOne(c => c.Project)
            .WithMany(p => p.Contracts)
            .HasForeignKey(c => c.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Client)
            .WithMany(cp => cp.Contracts)
            .HasForeignKey(c => c.ClientId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Freelancer)
            .WithMany(fp => fp.Contracts)
            .HasForeignKey(c => c.FreelancerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}