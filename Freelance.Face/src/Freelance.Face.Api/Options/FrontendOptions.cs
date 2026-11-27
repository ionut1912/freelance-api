namespace Freelance.Face.Api.Options;

public sealed class FrontendOptions
{
    public const string BaseUrlKey = "FRONTEND_BASE_URL";

    /// <summary>Base URL of the web application that hosts the remote capture page.</summary>
    [ConfigurationKeyName(BaseUrlKey)]
    public string? BaseUrl { get; set; }
}
