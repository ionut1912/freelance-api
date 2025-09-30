using Frelance.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frelance.Infrastructure.Context.Configurations;

public class TimeLogsConfiguration : BaseEntityConfiguration<TimeLogs>
{
    public override void Configure(EntityTypeBuilder<TimeLogs> builder)
    {
        base.Configure(builder);
    }
}
