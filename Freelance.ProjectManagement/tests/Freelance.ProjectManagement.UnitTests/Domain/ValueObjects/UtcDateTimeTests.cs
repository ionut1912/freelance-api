using Freelance.ProjectManagement.Domain.ValueObjects;

namespace Freelance.ProjectManagement.UnitTests.Domain.ValueObjects;

public sealed class UtcDateTimeTests
{
    private static readonly DateTime Clock = new(2028, 2, 29, 23, 30, 0);

    [Fact]
    public void From_Utc_IsUnchanged()
    {
        var value = DateTime.SpecifyKind(Clock, DateTimeKind.Utc);

        UtcDateTime.From(value).Should().Be(value).And.BeIn(DateTimeKind.Utc);
    }

    [Fact]
    public void From_Unspecified_KeepsTheClockValueAsUtc() =>
        UtcDateTime.From(Clock).Should().Be(Clock).And.BeIn(DateTimeKind.Utc);

    /// <summary>Expected value is derived from the machine's zone, so the test holds in any time zone.</summary>
    [Fact]
    public void From_Local_ConvertsToUtc()
    {
        var instant = new DateTimeOffset(2028, 2, 29, 23, 30, 0, TimeSpan.FromHours(5));

        UtcDateTime.From(instant.LocalDateTime).Should().Be(instant.UtcDateTime).And.BeIn(DateTimeKind.Utc);
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(3155378975999999999L)]
    public void From_ExtremeUnspecifiedValues_DoNotOverflow(long ticks) =>
        UtcDateTime.From(new DateTime(ticks)).Ticks.Should().Be(ticks);
}
