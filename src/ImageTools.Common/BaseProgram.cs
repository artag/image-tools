using ImageTools.Common;
using ImageTools.Interfaces;

namespace ImageTools.Implementation;

/// <summary>
/// Base program.
/// </summary>
/// <typeparam name="T">Options name.</typeparam>
/// <param name="appName">Application name.</param>
/// <param name="versionProvider">Application version provider.</param>
/// <param name="log">Logger.</param>
public abstract class BaseProgram<T>(
    string appName,
    IVersionProvider versionProvider,
    ILog log)
    where T : Enum
{
    /// <summary>
    /// Run program.
    /// </summary>
    /// <param name="args">Input arguments from CLI.</param>
    /// <param name="options">Command line options.</param>
    /// <returns>Asynchronous operation.</returns>
    public async Task Run(ICollection<string> args, ICollection<Option<T>> options)
    {
        var usage = new Usage(appName, versionProvider, log);
        var cli = new Arguments<T>();

        try
        {
            var opts = cli.Process(args);
            await Process(opts);
        }
        catch (OperationCanceledException)
        {
            log.LogInfo($"Shutdown {appName} application...");
        }
        catch (ImageToolsException ex)
        {
            log.LogError(ex.Message);
            usage.ShowUsage(options);
        }
        catch (Exception ex)
        {
            log.LogError(ex.Message);
        }
    }

    /// <summary>
    /// Process image(s) using options.
    /// </summary>
    /// <param name="options">Options.</param>
    /// <returns>Asynchronous operation.</returns>
    protected abstract Task Process(ICollection<Option<T>> options);
}
