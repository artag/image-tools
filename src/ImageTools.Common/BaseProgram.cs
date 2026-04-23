using ImageTools.Interfaces;

namespace ImageTools.Common;

/// <summary>
/// Base program.
/// </summary>
public abstract class BaseProgram
{
    private readonly string _appName;
    private readonly IVersionProvider _versionProvider;
    private readonly IFileSystem _fileSystem;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseProgram"/> class.
    /// </summary>
    /// <param name="appName">Application name.</param>
    /// <param name="versionProvider">Application version provider.</param>
    /// <param name="fileSystem">Common file system operations.</param>
    /// <param name="log">Logger.</param>
    protected BaseProgram(
        string appName,
        IVersionProvider versionProvider,
        IFileSystem fileSystem,
        ILog log)
    {
        _appName = appName;
        _versionProvider = versionProvider;
        _fileSystem = fileSystem;
        Log = log;
    }

    /// <summary>
    /// Logger.
    /// </summary>
    protected ILog Log { get; }

    /// <summary>
    /// Run program.
    /// </summary>
    /// <param name="args">Input arguments from CLI.</param>
    /// <param name="options">Command line options.</param>
    /// <returns>Asynchronous operation.</returns>
#pragma warning disable SP2101 // Method is too long
    public async Task Run(ICollection<string> args, ICollection<Option> options)
#pragma warning restore SP2101 // Method is too long
    {
        var usage = new Usage(_appName, _versionProvider, Log);
        var defaultOptions = CreateDefaultOptions();
        var allOptions = defaultOptions.Union(options).ToArray();

        try
        {
            await RunInternal(args, allOptions);
        }
        catch (OperationCanceledException)
        {
            Log.LogInfo($"Shutdown {_appName} application...");
        }
        catch (ImageToolsException ex)
        {
            Log.LogError(ex.Message);
            if (ex.Code is not ImageToolsExceptionCode.ErrorOnImageProcessing)
            {
                usage.ShowUsage(allOptions);
            }
        }
        catch (Exception ex)
        {
            Log.LogError(ex.Message);
        }
    }

    /// <summary>
    /// Process image(s) using options.
    /// </summary>
    /// <param name="path">Filename to process.</param>
    /// <param name="options">Options.</param>
    /// <returns>Asynchronous operation.</returns>
    protected abstract Task<bool> Process(string path, Options options);

    private static Arguments ConfigureArgumentsHandler(Option[] options)
    {
        var cli = new Arguments(options);
        cli.AddExclusiveGroup("-f", "-d", "-a");
        return cli;
    }

    private static Option[] CreateDefaultOptions()
    {
        const string groupName = "Application work mode options";
        return [
            new StringOption(WorkMode.ProcessFile)
            {
                Names = ["-f", "--file"],
                Description = "Process single file",
                GroupName = groupName,
            },
            new BooleanOption(WorkMode.ProcessFilesInDirectory)
            {
                Names = ["-d", "--dir"],
                Description = "Process files in current (single) directory",
                GroupName = groupName,
            },
            new BooleanOption(WorkMode.ProcessFilesInDirectortyRecursive)
            {
                Names = ["-a", "--alldir"],
                Description = "Process files in current directory and subdirectories",
                GroupName = groupName,
            },
        ];
    }

    private static Option GetWorkModeOption(Options options) =>
        options.GetOption(
            WorkMode.ProcessFile,
            WorkMode.ProcessFilesInDirectory,
            WorkMode.ProcessFilesInDirectortyRecursive);

    private async Task RunInternal(ICollection<string> args, Option[] opts)
    {
        var cli = ConfigureArgumentsHandler(opts);
        var options = cli.Process(args);
        var mode = GetWorkModeOption(options);
        if (mode.OptionName is WorkMode.ProcessFile)
        {
            var filename = mode.GetValue<string>();
            await ProcessFile(filename, options);
            return;
        }
        else if (mode.OptionName is WorkMode.ProcessFilesInDirectory)
        {
            var files = _fileSystem.GetFiles("*.jpg");
            await ProcessMultipleFiles(files, options);
            return;
        }
        else
        {
            var files = _fileSystem.GetFilesRecursive("*.jpg");
            await ProcessMultipleFiles(files, options);
        }
    }

    private async Task ProcessMultipleFiles(ICollection<string> files, Options options)
    {
        foreach (var file in files)
        {
            await ProcessFile(file, options);
        }
    }

    private async Task ProcessFile(string filename, Options options)
    {
        try
        {
            var proccessed = await Process(filename, options);
            if (proccessed)
            {
                Log.LogInfo($"Processed: {filename}");
            }
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (ImageToolsException ex)
        {
            Log.LogError(ex.Message);
        }
        catch (Exception ex)
        {
            Log.LogError(ex.Message);
        }
    }
}
