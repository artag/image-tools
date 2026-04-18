using System.Reflection;
using ImageTools.Interfaces;

namespace ImageTools.Implementation;

/// <summary>
/// Application version provider from assembly via reflection.
/// </summary>
public class VersionProvider : IVersionProvider
{
    /// <inheritdoc/>
    public string GetVersion() =>
        Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString(3) ?? string.Empty;
}
