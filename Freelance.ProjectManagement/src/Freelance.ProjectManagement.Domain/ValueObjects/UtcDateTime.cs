namespace Freelance.ProjectManagement.Domain.ValueObjects;

public static class UtcDateTime
{
    /// <summary>
    /// Local values (JSON input carrying an offset) are converted to UTC; values without any time zone
    /// information are taken to be UTC already.
    /// </summary>
    public static DateTime From(DateTime value) =>
        value.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
            : value.ToUniversalTime();
}
