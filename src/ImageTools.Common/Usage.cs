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
        ShotApplicationInfo();
        if (options is null || options.Count is 0)
        {
            return;
        }

        log.LogInfo("Usage:");
        ShowGroupedOptions(options);
        ShowStandaloneOptions(options);
    }

    private void ShotApplicationInfo()
    {
        var version = versionProvider.GetVersion();
        var info = string.IsNullOrEmpty(version)
            ? $"{appName}"
            : $"{appName}, version {version}";

        log.LogInfo(info);
    }

    private void ShowGroupedOptions(ICollection<Option> options)
    {
        var groups = options.Where(o => o.GroupName != null).GroupBy(o => o.GroupName);
        foreach (var group in groups)
        {
            log.LogInfo($"{group.Key}:");
            foreach (var opt in group)
            {
                log.LogInfo($"    {opt.ToLog()}");
            }
        }
    }

    private void ShowStandaloneOptions(ICollection<Option> options)
    {
        var standalone = options.Where(o => o.GroupName == null);
        foreach (var opt in standalone)
        {
            log.LogInfo(opt.ToLog());
        }
    }
}
