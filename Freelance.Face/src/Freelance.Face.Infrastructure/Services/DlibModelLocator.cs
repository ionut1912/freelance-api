namespace Freelance.Face.Infrastructure.Services;

/// <summary>Finds the dlib model files: next to the application, or in the repository during development.</summary>
public static class DlibModelLocator
{
    public static string Resolve(string fileName)
    {
        var defaultPath = Path.Combine(AppContext.BaseDirectory, "models", fileName);
        if (File.Exists(defaultPath)) return defaultPath;

        // Development fallback: walk up from bin/ to find the repository's models folder.
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir is not null; dir = dir.Parent)
        {
            string[] candidates = [Path.Combine(dir.FullName, "Freelance.Face", "models", fileName), Path.Combine(dir.FullName, "models", fileName)];
            if (candidates.FirstOrDefault(File.Exists) is { } found) return found;
        }

        throw new FileNotFoundException($"dlib model '{fileName}' was not found.", defaultPath);
    }
}
