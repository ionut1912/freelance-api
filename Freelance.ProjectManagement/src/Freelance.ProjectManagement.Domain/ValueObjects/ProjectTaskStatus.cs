using System.Diagnostics.CodeAnalysis;
using Shared.Domain.Common;

namespace Freelance.ProjectManagement.Domain.ValueObjects;

public sealed class ProjectTaskStatus : ValueObject
{
    public static readonly ProjectTaskStatus New = new("New");
    public static readonly ProjectTaskStatus InProgress = new("InProgress");
    public static readonly ProjectTaskStatus Review = new("Review");
    public static readonly ProjectTaskStatus Done = new("Done");

    public static IReadOnlyList<ProjectTaskStatus> All { get; } = [New, InProgress, Review, Done];

    private ProjectTaskStatus(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static bool IsValid(string? value) => TryFromName(value, out _);

    public static bool TryFromName(string? value, [NotNullWhen(true)] out ProjectTaskStatus? status)
    {
        status = All.FirstOrDefault(s => string.Equals(s.Value, value, StringComparison.Ordinal));
        return status is not null;
    }

    public static ProjectTaskStatus FromName(string value) =>
        TryFromName(value, out var status)
            ? status
            : throw new ArgumentOutOfRangeException(nameof(value), value, $"Unknown task status '{value}'.");

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
