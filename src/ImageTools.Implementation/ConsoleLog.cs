using ImageTools.Interfaces;

namespace ImageTools.Implementation;

/// <summary>
/// Implementation of ILog that outputs messages directly to the system console.
/// </summary>
public class ConsoleLog : ILog
{
    /// <inheritdoc/>
    public void LogInfo(string message)
    {
        Console.WriteLine(message);
    }

    /// <inheritdoc/>
    public void LogError(string message)
    {
        var formattedMessage = $"Error. {message}";
        Console.WriteLine(formattedMessage);
    }
}
