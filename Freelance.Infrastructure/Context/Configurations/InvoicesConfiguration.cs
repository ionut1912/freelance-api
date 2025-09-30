using Freelance.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freelance.Infrastructure.Context.Configurations;

public class InvoicesConfiguration : BaseEntityConfiguration<Invoices>
{
    public override void Configure(EntityTypeBuilder<Invoices> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Status).MaxLength100();
        builder.Property(p => p.InvoiceFile).MaxLength205000();
        builder.Property(p => p.Amount).HasPrecision(18, 2);

        builder.HasOne(i => i.Project)
            .WithMany(p => p.Invoices)
            .HasForeignKey(i => i.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(i => i.Client)
            .WithMany(c => c.Invoices)
            .HasForeignKey(i => i.ClientId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(i => i.Freelancer)
            .WithMany(fp => fp.Invoices)
            .HasForeignKey(i => i.FreelancerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}