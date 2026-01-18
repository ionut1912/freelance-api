using Freelance.ProjectManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freelance.ProjectManagement.Infrastructure.Persistance.Configurations;

public class TimeLogConfigurations : IEntityTypeConfiguration<TimeLog>
{
    public void Configure(EntityTypeBuilder<TimeLog> builder)
    {
        builder.HasKey(tl => tl.Id);
        builder.Property(tl => tl.Id)
            .ValueGeneratedNever();
    }
}
