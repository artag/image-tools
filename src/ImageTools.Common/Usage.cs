using ImageTools.Interfaces;

namespace ImageTools.Common;

/// <summary>
/// Usage info.
/// </summary>
/// <param name="appName">Name of the program.</param>
/// <param name="versionProvider">Application version provider.</param>
/// <param name="log">Logging implementation.</param>
public class Usage(
    string appName,
    IVersionProvider versionProvider,
    ILog log)
{
    /// <summary>
    /// Displays help message to the log.
    /// </summary>
    /// <param name="options">Command line options.</param>
    public void ShowUsage(ICollection<Option> options)
    {
        var version = versionProvider.GetVersion();
        var info = string.IsNullOrEmpty(version)
            ? $"{appName}"
            : $"{appName} v{version}";

        log.LogInfo(info);
        if (options is null || options.Count is 0)
        {
            return;
        }

        log.LogInfo("Usage:");
        foreach (var opt in options)
        {
            log.LogInfo(opt.ToLog());
        }
    }
}
