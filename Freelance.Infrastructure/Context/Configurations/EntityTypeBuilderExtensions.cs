using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freelance.Infrastructure.Context.Configurations;

public static class EntityTypeBuilderExtensions
{
    public static PropertyBuilder<string> MaxLength100(this PropertyBuilder<string> builder)
    {
        return builder.HasMaxLength(100);
    }

    public static PropertyBuilder<string> MaxLength205000(this PropertyBuilder<string> builder)
    {
        return builder.HasMaxLength(205000);
    }
}