using Frelance.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frelance.Infrastructure.Context.Configurations;

public class SkillsConfiguration : BaseEntityConfiguration<Skills>
{
    public override void Configure(EntityTypeBuilder<Skills> builder)
    {
        base.Configure(builder);
        builder.Property(p => p.ProgrammingLanguage).MaxLength100();
        builder.Property(p => p.Area).MaxLength100();

        builder.HasData(
            new Skills { Id = 1, ProgrammingLanguage = ".NET", Area = "Backend" ,CreatedAt = DateTime.UtcNow},
            new Skills { Id = 2, ProgrammingLanguage = "Angular", Area = "Frontend" ,CreatedAt = DateTime.UtcNow},
            new Skills { Id = 3, ProgrammingLanguage = "JavaScript", Area = "Frontend" ,CreatedAt = DateTime.UtcNow},
            new Skills { Id = 4, ProgrammingLanguage = "React", Area = "Frontend" ,CreatedAt = DateTime.UtcNow},
            new Skills { Id = 5, ProgrammingLanguage = "Python", Area = "Backend" ,CreatedAt = DateTime.UtcNow},
            new Skills { Id = 6, ProgrammingLanguage = "Java", Area = "Backend" ,CreatedAt = DateTime.UtcNow}
        );
    }
}
